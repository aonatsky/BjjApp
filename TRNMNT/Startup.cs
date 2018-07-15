using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Logger;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Web.Helpers;
using TRNMNT.Web.Hubs;

namespace TRNMNT.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional : false, reloadOnChange : true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<UserSecretsConfig>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region AppServices

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddAppServices();

            #endregion

            #region AppDBContext

            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
            services.AddIdentity<User, IdentityRole>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 1;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            #endregion

            //Authorization
            services.AddAuthenticationOptions(Configuration);

            services.AddSignalR();

            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug(LogLevel.Error);
            loggerFactory.AddLog4Net(Path.Combine(env.WebRootPath, "Config", "log4net.config"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                        HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
                routes.MapHub<RunEventHub>("/runevent");
                routes.MapHub<RoundHub>("/round-hub");
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{*all}",
                    defaults : new { controller = "Home", action = "Index" }
                );
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults : new { controller = "Home", action = "Index" });
            });

        }
    }
}