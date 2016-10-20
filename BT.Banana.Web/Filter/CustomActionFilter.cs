using BT.Banana.Web.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BT.Banana.Web.Filter
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var id = filterContext.ActionParameters["id"].ToString();
            if (FilterCache.Cache.IsContains(id))
            {
                filterContext.Result = new RedirectResult("/NotFound");
            }
        }
    }
}