using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TRNMNT.Web.WebSockets.Handlers;

namespace TRNMNT.Web.WebSockets
{
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder MapWebSocketHandlers(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            return applicationBuilder.MapWebSocketManager("/ws", serviceProvider.GetService<TestWebSocketHandler>());
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder applicationBuilder, PathString path, WebSocketHandler handler)
        {
            return applicationBuilder.Map(path, app => app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}