using BT.Banana.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BT.Banana.Web.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        public ActionResult SetLang()
        {
            var culture = RequestHelper.GetValue("lang");
            culture = CultureHelper.GetImplementedCulture(culture);
            var cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
    }
}