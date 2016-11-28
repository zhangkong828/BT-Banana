using BT.Banana.Web.Core;
using BT.Banana.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BT.Banana.Web.Controllers
{
    public class BaseController : Controller
    {
        public string cultureName;
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //先试图获取cookie中的语言设置，如果没有，则使用浏览器中带有的语言设置
            var cultureCookie = CookieHelper.GetValue("_culture");
            if (!string.IsNullOrEmpty(cultureCookie))
                cultureName = cultureCookie;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                Request.UserLanguages[0] : null;

            // 验证，如果为空或者不在当前本地化内，则使用默认语言
            cultureName = CultureHelper.GetImplementedCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            // 标记异常已处理
            filterContext.ExceptionHandled = true;
            var currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            //log
            Log.Error(currentUrl, filterContext.Exception);
            // 跳转到错误页
            filterContext.Result = new RedirectResult("/NotFound");
        }
    }
}