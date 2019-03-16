using Microsoft.AspNetCore.Builder;

namespace Brewdude.Application.Infrastructure
{
    public static class RequestHandlerExtension
    {
        public static IApplicationBuilder UseRequestHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestHandler>();
        }
    }
}