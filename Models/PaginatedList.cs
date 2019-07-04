using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Models
{
    public class PaginatedList<T>
    {
        /// <summary>
        /// Items
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Total items
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// The count per page
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// The zer-based page number
        /// </summary>
        public int? CurrentIndex { get; set; }

        /// <summary>
        /// A description for this list.
        /// </summary>
        public string Description { get; set; }
    }

}
