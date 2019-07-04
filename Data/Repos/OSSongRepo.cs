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

        public OSSongRepo(Data.SongDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public bool Exists(string filename)
        {
            var exists =  _context.OSSongs.Any(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));
            return exists;
        }

        public bool Exists(int id)
        {
            var exists = _context.OSSongs.Any(m => m.ID == id);
            return exists;
        }
        public async Task<OSSong> Get(int id)
        {
            var song = await _context.OSSongs.AsNoTracking()

                // TBD: do we really want/need these?
                // What we really want is a safer, stripped-down version of this information.
                .Include(s => s.CreatedBy)

                .SingleOrDefaultAsync(m => m.ID == id);

            return song;
        }

        
        public async Task<OSSong> GetByFilename(string filename)
        {
            var song = await _context.OSSongs.AsNoTracking()

               // TBD: do we really want/need these?
               .Include(s => s.CreatedBy)

               .SingleOrDefaultAsync(m => m.Filename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));

            return song;
        }

        public async Task<PaginatedList<SongBrief>> Page(SongFilterParameter queryParameter, PagingParameter pagingParameter)
        {
            var query =  _context.OSSongs
                .Include(s => s.CreatedBy)
                .AsNoTracking();

            if (queryParameter != null)
            {
                if (!string.IsNullOrEmpty(queryParameter.author))
                {
                    query = query.Where(s => s.Author.Contains(queryParameter.author));
                }
                if (!string.IsNullOrEmpty(queryParameter.title))
                {
                    query = query.Where(s => s.Title.Contains(queryParameter.title));
                }
                if (!string.IsNullOrEmpty(queryParameter.lyrics))
                {
                    query = query.Where(s => s.Content.Contains(queryParameter.lyrics));
                }
            }

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

        public async Task<List<OSSong>> All(SongFilterParameter queryParameter)
        {
            var query = _context.OSSongs
                .Include(s => s.CreatedBy)
                .AsNoTracking();

            if (queryParameter != null)
            {
                if (!string.IsNullOrEmpty(queryParameter.author))
                {
                    query = query.Where(s => s.Author.Contains(queryParameter.author));
                }
                if (!string.IsNullOrEmpty(queryParameter.title))
                {
                    query = query.Where(s => s.Title.Contains(queryParameter.title));
                }
                if (!string.IsNullOrEmpty(queryParameter.lyrics))
                {
                    query = query.Where(s => s.Content.Contains(queryParameter.lyrics));
                }
            }

            query = query.OrderBy(s => s.Title);

            return await query.ToListAsync();
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

            return song;
        }

        public OSSong Update(OSSong OSSong)
        {
            _context.Update(OSSong);
            return OSSong;
        }
    }
}
