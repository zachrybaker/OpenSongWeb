using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenSongWeb.Models;

namespace OpenSongWeb.Data.Repos
{
    /// <summary>
    /// Interface for In-Memory access to OSSong
    /// </summary>
    public interface IOSSongRepo
    {
        /// <summary>
        /// Get paginated songs, optionally filtered and paged.
        /// </summary>
		/// <param name="queryParameter">search criteria</param>
		/// <param name="pagingParameter">Pagination settings (limit, offset, columnToOrder, columnOrder)</param>
        /// <returns></returns>
        Task<PaginatedList<SongBrief>> Page(SongFilterParameter queryParameter, PagingParameter pagingParameter);

        /// <summary>
        /// Get all songs, optionally filtered.
        /// </summary>
        /// <param name="queryParameter">search criteria</param>
        /// <returns>A list of songs, or an ErrorInfo</returns>
        Task<IEnumerable<OSSong>> All(SongFilterParameter queryParameter);

        /// <summary>
        /// Get an OpenSong Song by Id
        /// </summary>
        /// <param name="id">song ID</param>
        /// <returns>song, or null</returns>
        Task<OSSong> Get(int id);

        /// <summary>
        /// Get an OpenSong Song by filename (which is unique)
        /// </summary>
        /// <param name="filename">song filename</param>
        /// <returns>song, or null</returns>
        Task<OSSong> GetByFilename(string filename);

        /// <summary>
        /// Determine if a song is already in the database by a filename (which is unique)
        /// </summary>
        /// <param name="filename">The filename the song came in as (usually matches the title, with special characters removed</param>
        /// <returns>true if already in db.</returns>
        Task<bool> Exists(string filename);

        Task<bool> Exists(int id);

        /// <summary>
        /// Create a new OpenSong Song, returns the newly created OpenSong Song
        /// </summary>
        /// <param name="OSSong">OpenSong Song to be created</param>
        /// <returns>OSSong</returns>
        OSSong Create(OSSong OSSong);

        /// <summary>
        /// Update an existing OpenSong Song.
        /// Returns the OpenSong Song.
        /// </summary>
        /// <param name="OSSong">OpenSong Song to be updated</param>
        /// <returns>OSSong</returns>
        OSSong Update(OSSong OSSong);
    }
}
