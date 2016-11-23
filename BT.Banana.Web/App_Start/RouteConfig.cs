using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BT.Banana.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //自定义路由
            routes.MapRoute(
                name: "s-page",
                url: "s/{key}__{type}_{index}",
                defaults: new { controller = "Home", action = "S", key = UrlParameter.Optional, type = UrlParameter.Optional, index = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "s",
             url: "s/{key}",
             defaults: new { controller = "Home", action = "S", key = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "d",
                url: "d/{id}.html",
                defaults: new { controller = "Home", action = "D", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
