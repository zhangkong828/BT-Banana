using BT.Banana.Web.Core;
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
        /// <summary>
        /// 首页
        /// </summary>
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
            var redirecturl = Request.UrlReferrer.ToString();
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
            return Redirect(redirecturl);
        }

        /// <summary>
        /// 搜索页
        /// </summary>
        public ActionResult S(string key, string index)
        {
            if (string.IsNullOrEmpty(key))
                return RedirectToAction("index");
            var currentIndex = 0;
            if (!int.TryParse(index, out currentIndex))
                currentIndex = 1;
            currentIndex = currentIndex < 1 ? 1 : currentIndex;
            var result = Engiy_Com.Search(key, currentIndex);
            return View(result);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        public ActionResult D()
        {
            return View();
        }
    }
}