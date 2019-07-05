using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using OpenSongWeb.Data;
using OpenSongWeb.Data.Repos;
using OpenSongWeb.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenSongWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.SongDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("WorshipDB")));

            services.AddDefaultIdentity<AppUser>()
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<Data.SongDbContext>();

            services.AddMvc();
            // services.AddCors();

            // Enables caching in the repo layer, keeping songs in memory for faster operation.  
            // Can be disabled by "CacheSongs" in the app configuration.
            services.AddMemoryCache();

            // Scoped = don't make multiple instantiations per [request] life cycle.
            //  i.e. connections to a Repo per web request
            services.AddScoped<IRepoUnitOfWork, RepoUnitOfWork>();
            services.AddScoped<IOSSongRepo, OSSongRepo>();

            // Transient = doesn't hold any data between uses.
            services.AddTransient<IXMLDataImportManager, XMLDataImportManager>();

            services.AddTransient<IOSSongManager, OSSongManager>();

            // Singleton = holds data that should be available all the time, in one spot.
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IKeyProcessor, KeyProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // app.UseDefaultFiles();  
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            // coming soon..app.UseAuthentication();

            // app.UseAuthorization(); // a 3.0 method?

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // On boot we always want to try to import data.
                // We don't really care to do it more than once while running though.
                var xmlDataImportManager = scope.ServiceProvider.GetRequiredService<IXMLDataImportManager>();
                xmlDataImportManager.PerformImport();
            }

        }
    }
}
