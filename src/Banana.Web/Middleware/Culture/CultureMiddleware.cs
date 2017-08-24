using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Middleware.Culture
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var cultureCookieName = httpContext.Request.Cookies["_culture"];
            //如果为空或者不在当前本地化内，则使用默认语言
            var cultureName = CultureHelper.GetImplementedCulture(cultureCookieName);


            await _next.Invoke(httpContext);
        }
    }
}
