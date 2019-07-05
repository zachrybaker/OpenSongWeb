using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Data
{
    /// <summary>
	/// Paging parameter properties for limit, offset, and ordering
	/// </summary>
	public class PagingParameter
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string columnToOrder { get; set; }
        public string columnOrder { get; set; }
    }

    public enum SongSearchType
    {
        All,
        Title,
        Author,
        Lyrics,
        Tags
    }

    /// <summary>
    /// Client-facing representation of song search parameters
    /// </summary>
    public class SongSearchParameters
    {
        public string Text { get; set; }
        public SongSearchType type { get; set; }
    }

    /// <summary>
    /// Allow for queries to the database to match any (or) or all (and)
    /// </summary>
    public enum SearchCriteriaBuildType
    {
        Any,
        All,
        Exact //(And, all string match exactly).  seems too tight.
    }


    /// <summary>
    /// Query/filter properties for songs.
    /// </summary>
    public class SongFilterParameter
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Lyrics { get; set; }

        public string Themes { get; set; }

        public SearchCriteriaBuildType SearchCriteriaBuildType { get; set; }
    }

}
