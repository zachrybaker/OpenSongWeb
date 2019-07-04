using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Managers
{
    public interface IXMLDataImportManager
    {
        /// <summary>
		/// Set up a data import process to be run in the background
		/// </summary>
		void EnqueueBackgroundImport();

        /// <summary>
        /// Process all data currently set up to be imported.
        /// </summary>
        /// <returns></returns>
        Task PerformImport();
        
    }
}
