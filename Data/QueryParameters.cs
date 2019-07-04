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
        Lyrics
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
    /// Query/filter properties for songs.
    /// </summary>
    public class SongFilterParameter
    {
        public string title { get; set; }
        public string author { get; set; }

        public string lyrics { get; set; }
    }

}
