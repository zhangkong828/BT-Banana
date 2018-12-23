using Microsoft.AspNetCore.Mvc.Filters;

namespace Banana.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var path = context.HttpContext.Request.Path;
            var queryString = context.HttpContext.Request.QueryString.Value;
            Logger.Fatal(context.Exception, $"[url]:{path + queryString}\r\n[controller]:{controller}\r\n[action]:{action}");
        }
    }
}
