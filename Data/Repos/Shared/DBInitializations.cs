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
        public static void Initialize(SongDbContext context, bool performMigrations = false, bool performConfiguration = true)
        {
            if (performMigrations)
            {
                context.Database.Migrate();
            }

            if (performConfiguration)
            {
                if(!context.AppConfigurations.Any(c => c.Name == ConfigKeys.OpenIddict.ClientId))
                {
                    context.AppConfigurations.Add(new AppConfiguration { Name = ConfigKeys.OpenIddict.ClientId, Value = Guid.NewGuid().ToString("N") });
                    context.AppConfigurations.Add(new AppConfiguration { Name = ConfigKeys.OpenIddict.ClientSecret, Value = Helpers.RandomGenerator.GenerateString(128) }); ;
                    context.SaveChanges();

                }

            }
        }
    }
}
