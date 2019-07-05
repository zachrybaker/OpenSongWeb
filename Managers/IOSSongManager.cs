using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSongWeb.Data;
using OpenSongWeb.Models;

namespace OpenSongWeb.Managers
{
    public interface IOSSongManager
    {
        /// <summary>
        /// Get a song detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Either the OSSong, or error info </returns>
        Task<Either<OSSong, ErrorInfo>> Get(int Id);

        /// <summary>
        /// Get paginated songs, optionally filtered and paged.
        /// </summary>
		/// <param name="queryParameter">search criteria</param>
		/// <param name="pagingParameter">Pagination settings (limit, offset, columnToOrder, columnOrder)</param>
        /// <returns>A paginated list structure of songs, or an ErrorInfo</returns>
        Task<Either<PaginatedList<SongBrief>, ErrorInfo>> Page(SongFilterParameter queryParameter, PagingParameter pagingParameter);

        /// <summary>
        /// Get all songs, optionally filtered.
        /// </summary>
        /// <param name="queryParameter">search criteria</param>
        /// <returns>A list of songs, or an ErrorInfo</returns>
        Task<Either<List<SongBrief>, ErrorInfo>> All(SongFilterParameter queryParameter);

        Task<bool> Exists(int id);
    }
}
