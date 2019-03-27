using Brewdude.Middleware.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace Brewdude.Web.Infrastructure
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}