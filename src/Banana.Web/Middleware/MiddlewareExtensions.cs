using Banana.Web.Middleware.Culture;
using Microsoft.AspNetCore.Builder;

namespace Banana.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder builder)
           => builder.UseMiddleware<CultureMiddleware>();
    }
}
