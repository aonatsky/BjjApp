using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TRNMNT.Core.Configurations;
using TRNMNT.Core.Configurations.Impl;
using TRNMNT.Core.Helpers.Impl;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Model.FileProcessingOptions;
using TRNMNT.Core.Services.impl;
using TRNMNT.Core.Services.Impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.Repositories.Impl;

namespace TRNMNT.Web.Helpers
{
    public static class AppExpensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
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
            services.AddScoped(typeof(IBracketService), typeof(BracketService));
            services.AddScoped(typeof(IResultsService), typeof(ResultsService));
            services.AddScoped(typeof(IMatchService), typeof(MatchService));
            services.AddScoped(typeof(IParticipantRegistrationService), typeof(ParticipantRegistrationService));
            services.AddScoped(typeof(IAuthConfiguration), typeof(AuthConfiguration));

            services.AddScoped<IPaidServiceFactory, PaidServiceFactory>();

            return services;
        }
        public static IServiceCollection AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = services.BuildServiceProvider();
            var authConfig = sp.GetService<IAuthConfiguration>();
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authConfig.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = authConfig.Key,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });
//                        .AddFacebook(facebookOptions =>
//                {
//                    facebookOptions.AppId = configuration["Facebook:AppId"];
//                    facebookOptions.AppSecret = configuration["Facebook:AppSecret"];
//                });
            return services;
        }
    }
}