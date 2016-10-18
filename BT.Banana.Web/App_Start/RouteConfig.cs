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
                url: "d/{hash}.html",
                defaults: new { controller = "Home", action = "D", hash = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
