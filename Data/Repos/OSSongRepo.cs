using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using OpenSongWeb.Models;

namespace OpenSongWeb.Data.Repos
{
    public class OSSongRepo : IOSSongRepo
    {
        private readonly Data.SongDbContext _context;

        public readonly IMemoryCache _memoryCache;
        readonly bool _useCache;
        const string _SONGS_KEY = "AllSongs";

        public OSSongRepo(Data.SongDbContext context, IMemoryCache memoryCache, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _memoryCache = memoryCache;
            _useCache = true;
            bool.TryParse(configuration.GetSection("AppBehavior")?["CacheSongs"], out _useCache);
        }

        public async Task<bool> Exists(string filename)
        {
            if (_useCache)
            {
                return (await _GetCache()).Any(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return _context.OSSongs.Any(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public async Task<bool> Exists(int id)
        {
            if (_useCache)
            {
                return (await _GetCache()).Any(m => m.ID == id);
            }
            else
            {
                return _context.OSSongs.Any(m => m.ID == id);
            }
        }
        public async Task<OSSong> Get(int id)
        {
            if (_useCache)
            {
                return (await _GetCache()).SingleOrDefault(s => s.ID == id);
            }
            else
            {
                return await _context.OSSongs.AsNoTracking()

                // TBD: do we really want/need these?
                // What we really want is a safer, stripped-down version of this information.
                .Include(s => s.CreatedBy)

                .SingleOrDefaultAsync(m => m.ID == id);
            }
        }

        
        public async Task<OSSong> GetByFilename(string filename)
        {
            if (_useCache)
            {
                return (await _GetCache()).SingleOrDefault(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return await _context.OSSongs.AsNoTracking()
                    // TBD: do we really want/need these?
                    .Include(s => s.CreatedBy)
                    .SingleOrDefaultAsync(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public async Task<PaginatedList<SongBrief>> _Page_fromDB(SongFilterParameter queryParameter, PagingParameter pagingParameter)
        {
            var query = _context.OSSongs
                .Include(s => s.CreatedBy)
                .AsNoTracking();

            _SetIQueryableWhereConditions(query, queryParameter);

            var count = await query.CountAsync();

            if (pagingParameter != null)
            {
                bool bIsAscending = (pagingParameter.columnOrder?.ToLower() ?? "asc") == "asc" ? true : false;
                pagingParameter.columnToOrder = pagingParameter.columnToOrder?.ToLower() ?? "title";

                switch (pagingParameter?.columnToOrder)
                {
                    case "title":
                    default:
                        query = bIsAscending ? query.OrderBy(s => s.Title) : query.OrderByDescending(s => s.Title);
                        break;
                    // TODO: other cases as needed.
                }

                //Apply paging
                //Offset before limit!
                if (pagingParameter.offset != null)
                {
                    query = query.Skip((int)pagingParameter?.offset);
                }

                if (pagingParameter.limit != null)
                {
                    query = query.Take((int)pagingParameter?.limit);
                }
            }

            var songs = await query.ToListAsync();
            return new PaginatedList<SongBrief>
            {
                Items = songs.Select(s => new SongBrief(s)).ToList(),
                Total = count,
                CurrentIndex = pagingParameter?.offset ?? 0,
                PageSize = pagingParameter?.limit ?? 0
            };
        }

        public async Task<PaginatedList<SongBrief>> _Page_fromCache(SongFilterParameter queryParameter, PagingParameter pagingParameter)
        {
            IEnumerable<OSSong> songs = _FilterSongs(await _GetCache(), queryParameter);
            var count = songs.Count();

            if (pagingParameter != null)
            {
                bool bIsAscending = (pagingParameter.columnOrder?.ToLower() ?? "asc") == "asc" ? true : false;
                pagingParameter.columnToOrder = pagingParameter.columnToOrder?.ToLower() ?? "title";

                switch (pagingParameter?.columnToOrder)
                {
                    case "title":
                    default:
                        songs = bIsAscending ? songs.OrderBy(s => s.Title) : songs.OrderByDescending(s => s.Title);
                        break;
                        // TODO: other cases as needed.
                }

                //Apply paging
                //Offset before limit!
                if (pagingParameter.offset != null)
                {
                    songs = songs.Skip((int)pagingParameter?.offset);
                }

                if (pagingParameter.limit != null)
                {
                    songs = songs.Take((int)pagingParameter?.limit);
                }
            }

            return new PaginatedList<SongBrief>
            {
                Items = songs.Select(s => new SongBrief(s)).ToList(),
                Total = count,
                CurrentIndex = pagingParameter?.offset ?? 0,
                PageSize = pagingParameter?.limit ?? 0
            };
        }

        public async Task<PaginatedList<SongBrief>> Page(SongFilterParameter queryParameter, PagingParameter pagingParameter)
        {
            if (_useCache)
            {
                return await _Page_fromCache(queryParameter, pagingParameter);
            }
            else
            {
                return await _Page_fromDB(queryParameter, pagingParameter);
            }
        }

        protected void _SetIQueryableWhereConditions(IQueryable<OSSong> query, SongFilterParameter queryParameter)
        {
            if (queryParameter != null)
            {
                if (queryParameter.SearchCriteriaBuildType == SearchCriteriaBuildType.Any)
                {
                    query = query.Where(s => 
                    (!string.IsNullOrEmpty(queryParameter.Author) && s.Author.Contains(queryParameter.Author)) ||
                    (!string.IsNullOrEmpty(queryParameter.Title) && s.Title.Contains(queryParameter.Title)) ||
                    (!string.IsNullOrEmpty(queryParameter.Lyrics) && s.Content.Contains(queryParameter.Lyrics)) ||
                    (!string.IsNullOrEmpty(queryParameter.Themes) && s.Themes.Contains(queryParameter.Themes)));
                }
                else if (queryParameter.SearchCriteriaBuildType == SearchCriteriaBuildType.All)
                {
                    if (!string.IsNullOrEmpty(queryParameter.Author))
                    {
                        query = query.Where(s => s.Author != null && s.Author.Contains(queryParameter.Author));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Title))
                    {
                        query = query.Where(s => s.Title != null && s.Title.Contains(queryParameter.Title));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Lyrics))
                    {
                        query = query.Where(s => s.Content != null && s.Content.Contains(queryParameter.Lyrics));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Themes))
                    {
                        query = query.Where(s => s.Themes != null && s.Themes.Contains(queryParameter.Themes));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(queryParameter.Author))
                    {
                        query = query.Where(s => s.Author != null && s.Author.Equals(queryParameter.Author, StringComparison.InvariantCultureIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Title))
                    {
                        query = query.Where(s => s.Title != null && s.Title.Equals(queryParameter.Title, StringComparison.InvariantCultureIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Lyrics))
                    {
                        query = query.Where(s => s.Content != null && s.Content.Equals(queryParameter.Lyrics, StringComparison.InvariantCultureIgnoreCase));
                    }
                    if (!string.IsNullOrEmpty(queryParameter.Themes))
                    {
                        query = query.Where(s => s.Themes != null && s.Themes.Equals(queryParameter.Themes, StringComparison.InvariantCultureIgnoreCase));
                    }
                }
            }
        }

        protected async Task<IEnumerable<OSSong>> _All_fromDB(SongFilterParameter queryParameter)
        {
            var query = _context.OSSongs
                .Include(s => s.CreatedBy)
                .AsNoTracking();

            _SetIQueryableWhereConditions(query, queryParameter);

            query = query.OrderBy(s => s.Title);

            return await query.ToListAsync();
        }
        protected async Task<IEnumerable<OSSong>> _GetCache()
        {
            if (!_memoryCache.TryGetValue<IEnumerable<OSSong>>(_SONGS_KEY, out IEnumerable<OSSong> songs))
            {
                songs = await _All_fromDB(null);
                _memoryCache.Set(_SONGS_KEY, songs);
            }
            
            return songs;
        }

        public IEnumerable<OSSong> _FilterSongs(IEnumerable<OSSong> songs, SongFilterParameter queryParameter)
        {
            if (queryParameter != null)
            {
                if (queryParameter.SearchCriteriaBuildType == SearchCriteriaBuildType.Any)
                {
                    return songs.Where(s => 
                        (!string.IsNullOrEmpty(queryParameter.Author) && s.Author != null && s.Author.Contains(queryParameter.Author)) ||
                        (!string.IsNullOrEmpty(queryParameter.Title)  && s.Title != null && s.Title.Contains(queryParameter.Title)) ||
                        (!string.IsNullOrEmpty(queryParameter.Lyrics) && s.Content != null && s.Content.Contains(queryParameter.Lyrics)) ||
                        (!string.IsNullOrEmpty(queryParameter.Themes) && s.Themes != null && s.Themes.Contains(queryParameter.Themes))
                        );
                }
                else if (queryParameter.SearchCriteriaBuildType == SearchCriteriaBuildType.All)
                {
                    return songs.Where(s => 
                        (string.IsNullOrEmpty(queryParameter.Author) || (s.Author != null && s.Author.Contains(queryParameter.Author))) &&
                        (string.IsNullOrEmpty(queryParameter.Title)  || (s.Title != null && s.Title.Contains(queryParameter.Title))) &&
                        (string.IsNullOrEmpty(queryParameter.Lyrics) || (s.Content != null && s.Content.Contains(queryParameter.Lyrics))) &&
                        (string.IsNullOrEmpty(queryParameter.Themes) || (s.Themes != null && s.Themes.Contains(queryParameter.Themes)))
                        );
                }
                else
                {
                    return songs.Where(s =>
                       (string.IsNullOrEmpty(queryParameter.Author) || (s.Author != null && s.Author.Equals(queryParameter.Author, StringComparison.InvariantCultureIgnoreCase))) &&
                        (string.IsNullOrEmpty(queryParameter.Title) || (s.Title != null && s.Title.Equals(queryParameter.Title, StringComparison.InvariantCultureIgnoreCase))) &&
                        (string.IsNullOrEmpty(queryParameter.Lyrics) || (s.Content != null && s.Content.Equals(queryParameter.Lyrics, StringComparison.InvariantCultureIgnoreCase))) &&
                        (string.IsNullOrEmpty(queryParameter.Themes) || (s.Themes != null && s.Themes.Equals(queryParameter.Themes, StringComparison.InvariantCultureIgnoreCase)))
                       );
                }
            }

            return songs;
        }

        public async Task<IEnumerable<OSSong>> All(SongFilterParameter queryParameter)
        {
            if (_useCache)
            {
                return _FilterSongs((await _GetCache()), queryParameter);
            }
            else
            {
                return await _All_fromDB(queryParameter);
            }
        }

        public OSSong Create(OSSong song)
        {
            Debug.Assert(!song.ID.HasValue);
            if (song.ID.HasValue)
            {
                throw new ArgumentException("DB object being created should not have its ID set.");
            }

            _context.Add(song); // Sets ID properties
            // Save is handled by RepositoryUnitOfWork

            if(_useCache && _memoryCache.TryGetValue<IEnumerable<OSSong>>(_SONGS_KEY, out IEnumerable<OSSong> songs))
            {
                songs.Append(song);
                songs.OrderBy(s => s.Title); // we always sort the thing by title so that sorting later is not needed.
                _memoryCache.Set("AllSongs", songs);
            }

            return song;
        }

        public OSSong Update(OSSong song)
        {
            _context.Update(song);
            // Save is handled by RepositoryUnitOfWork

            if (_useCache && _memoryCache.TryGetValue<IEnumerable<OSSong>>(_SONGS_KEY, out IEnumerable<OSSong> songs))
            {
                var songList = (List<OSSong>)songs;
                for(var i = 0; i < songs.Count(); i++)
                {
                    if (songList[i].ID == song.ID)
                    {
                        songList[i] = song;
                    }
                }

                songList.OrderBy(s => s.Title); // we always sort the thing by title so that sorting later is not needed.
                _memoryCache.Set("AllSongs", songList);
            }

            return song;
        }
    }
}
