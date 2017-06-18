using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Context;
using TRNMNT.Web.Core.Services;
using TRNMNT.Web.Core.Services.impl;
using TRNMNT.Data.Repositories;
using Microsoft.Extensions.Logging;
using TRNMNT.Web.Core.Logger;
using System.IO;
using TRNMNT.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using TRNMNT.Web.Core.Authentication;

namespace TRNMNT.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            #region AppDBContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
            services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>();
            #endregion

            #region AppServices
            services.AddScoped(typeof(IFighterService), typeof(FighterService));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(FighterFileService));
            services.AddScoped(typeof(BracketsFileService));
            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net(Path.Combine(env.WebRootPath,"Config", "log4net.config"));
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

            var options = new JwtBearerOptions
            {

                TokenValidationParameters = {
                   ValidIssuer = TokenAuthOption.ISSUER,
                   ValidAudience = TokenAuthOption.AUDIENCE,
                   ValidateIssuer = true,
                   IssuerSigningKey = TokenAuthOption.GetKey(),
                   ValidateIssuerSigningKey = true,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero
                }
            };

            app.UseJwtBearerAuthentication(options);

        }
    }
}
;