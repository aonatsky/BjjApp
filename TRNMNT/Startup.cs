using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TRNMNT.Core.Configurations;
using TRNMNT.Core.Configurations.Impl;
using TRNMNT.Core.Helpers.Impl;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Logger;
using TRNMNT.Core.Services.impl;
using TRNMNT.Core.Model.FileProcessingOptions;
using TRNMNT.Core.Services.Impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.Repositories.Impl;

namespace TRNMNT.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.home.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region AppServices

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped(typeof(IParticipantProcessingService), typeof(ParticipantProcessingService));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IFileProcessiongService<ParticipantListProcessingOptions>, ParticipantListFileService>();
            services.AddScoped(typeof(BracketsFileService));
            services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
            services.AddScoped(typeof(IEventService), typeof(EventService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IParticipantService), typeof(ParticipantService));
            services.AddScoped(typeof(ITeamService), typeof(TeamService));
            services.AddScoped(typeof(IWeightDivisionService), typeof(WeightDivisionService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IFileService), typeof(LocalFileService));
            services.AddScoped(typeof(IPaymentService), typeof(LiqPayService));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IPromoCodeService), typeof(PromoCodeService));
            services.AddScoped(typeof(IRoundService), typeof(RoundService));
            services.AddScoped(typeof(IBracketService), typeof(BracketService));
            services.AddScoped(typeof(IParticipantRegistrationService), typeof(ParticipantRegistrationService));
            services.AddScoped(typeof(IAuthConfiguration), typeof(AuthConfiguration));
            services.AddScoped<IPaidServiceFactory, PaidServiceFactory>();

            #endregion

            //Authorization
            var authConfig = (IAuthConfiguration)services.FirstOrDefault(x => x.ServiceType == typeof(IAuthConfiguration))?.ImplementationInstance;
            services.AddAuthentication(o => o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = authConfig.Issuer,
                        ValidAudience = authConfig.Audience,
                        ValidateIssuer = true,
                        IssuerSigningKey = authConfig.Key,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.FromResult(0);
                        },
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HandleResponse();
                            return Task.FromResult(0);
                        }
                    };
                });

            // Add framework services.
            services.AddMvc();

            #region AppDBContext

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 1;
            }
                )
               .AddEntityFrameworkStores<AppDbContext>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{*all}",
                    defaults: new { controller = "Home", action = "Index" }
                    );
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
;