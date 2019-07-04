using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Managers
{
    /// <summary>
    /// A helper to assist in key processing.
    /// This thing is surely really buggy, I did not write this based on sound music theory.  
    /// And it is a port of some really old work of mine.
    /// TODO: Find something better.
    /// </summary>
    public interface IKeyProcessor
    {

        bool ParseKey(string strKeyIn, out string strTheKey, out string strNoteSuffix);


    }
}
