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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using AspNetCore.Firebase.Authentication.Extensions;

namespace OpenSongWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.SongDbContext>(options =>
            {
                // Configure the context to use an in-memory store (for testing)
                //options.UseInMemoryDatabase(nameof(SongDbContext));
                options.UseSqlServer(Configuration.GetConnectionString("WorshipDB"));

            });

            services.AddFirebaseAuthentication(
                Configuration.GetValue<string>("JWTAuthIssuer"),
                Configuration.GetValue<string>("JWTAuthAudience"));

            services.AddMvc();

            /*services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<Data.SongDbContext>()
                .AddDefaultTokenProviders();*/
                       
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
            // app.UseAuthorization(); // a 3.0 method?
            app.UseAuthentication();

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
                app.UseExceptionHandler("/Songs/Error");
            }

            // app.UseDefaultFiles();  
            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "spa-route",
                    template: "{controller}/{*anything=Index}",
                    defaults: new { controller = "Songs", action = "Index" });

                routes.MapRoute(
                   name: "app-fallback",
                   template: "{*anything}/",
                   defaults: new { controller = "Songs", action = "Index" });
            });

            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {

            using (var scope = services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SongDbContext>();
                await DBInitializations.Initialize(dbContext,
                    cancellationToken,
                    Configuration.GetValue<bool>("RunMigrationsOnBoot"),
                    Configuration.GetValue<bool>("PerformDBConfigurations"));

                // On boot we always want to try to import data.
                await scope.ServiceProvider.GetRequiredService<IXMLDataImportManager>().PerformImport();

            }
        }
    }
}
