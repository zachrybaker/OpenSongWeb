using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSongWeb.Data.Repos
{
    /// <summary>
    /// A place to run migrations at startup time
    /// </summary>
    public class DBInitializations
    {
        public static void Initialize(SongDbContext context)
        {
            context.Database.Migrate();


        }
    }
}
