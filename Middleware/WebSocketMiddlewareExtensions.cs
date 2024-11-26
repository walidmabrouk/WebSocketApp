using Microsoft.Extensions.DependencyInjection;

namespace WebSocketApp.Middleware
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IServiceCollection AddWebSocketMiddleware(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketServerConnectionManager>();
            return services;
        }

        public static IApplicationBuilder UseWebSocketMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }
    }
}