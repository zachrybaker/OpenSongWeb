using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSongWeb.Models;
using OpenSongWeb.Data;

[assembly: HostingStartup(typeof(OpenSongWeb.Areas.Identity.IdentityHostingStartup))]
namespace OpenSongWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
           /* builder.ConfigureServices((context, services) => {
                services.AddDbContext<SongDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddDefaultIdentity<AppUser>()
                    .AddEntityFrameworkStores<SongDbContext>();
            });*/
        }
    }
}
