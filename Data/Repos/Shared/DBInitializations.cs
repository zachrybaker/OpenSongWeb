using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSongWeb.Data.Repos
{
    /// <summary>
    /// A place to run migrations at startup time
    /// </summary>
    public class DBInitializations
    {
        public static async Task Initialize(SongDbContext context, CancellationToken cancellationToken, bool performMigrations = false, bool performConfiguration = true)
        {
            if (performMigrations)
            {
                await context.Database.MigrateAsync(cancellationToken);
            }

            if (performConfiguration)
            {
                
            }
        }
    }
}
