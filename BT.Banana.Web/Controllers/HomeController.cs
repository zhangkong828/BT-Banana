using BT.Banana.Web.Cache;
using BT.Banana.Web.Core;
using BT.Banana.Web.Filter;
using BT.Banana.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            CookieHelper.SetValue("_culture", culture);
            return Redirect(redirecturl);
        }

        /// <summary>
        /// 搜索页
        /// </summary>
        public ActionResult S(string key, string index)
        {
            if (string.IsNullOrEmpty(key))
                return RedirectToAction("index");
            key = key.Trim();//去掉前后空格
            var currentIndex = 0;
            if (!int.TryParse(index, out currentIndex))
                currentIndex = 1;
            currentIndex = currentIndex < 1 ? 1 : currentIndex;
            //使用Cache
            var cachekey = $"{key}_{cultureName}_{currentIndex}";//关键词_语言_页码
            var result = SearchCache.Cache.Get(cachekey);
            if (result == null)
            {
                result = Engiy_Com.Search(key, currentIndex, cultureName);
                SearchCache.Cache.Set(cachekey, result);
            }
            //var result = Engiy_Com.Search(key, currentIndex, cultureName);
            return View(result);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        [CustomActionFilter]
        public ActionResult D(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("NotFound");
            //使用Cache
            var cachekey = $"{id}_{cultureName}";//id_语言
            var result = DetailCache.Cache.Get(cachekey);
            if (result == null)
            {
                result = Engiy_Com.GetDetial(id, cultureName);
                DetailCache.Cache.Set(cachekey, result);
            }
            //var result = Engiy_Com.GetDetial(id, cultureName);
            return View(result);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        /// <summary>
        /// 百度电影风云榜
        /// </summary>
        public ActionResult GetBaiDuHotKeys()
        {
            var num = RequestHelper.GetInt("n");
            List<string> list = new List<string>();
            try
            {
                string url = "http://top.baidu.com/buzz?b=26&c=1&fr=topcategory_c1";
                var html = HttpHelper.Get(url,null, "GB2312");
                MatchCollection r = Regex.Matches(html, "<tr[\\s\\S]*?<td[\\s\\S]*?keyword\">[\\s\\S]*?>(.+?)<");
                int i = 0;
                foreach (Match item in r)
                {
                    if (i == num)
                    {
                        break;
                    }
                    list.Add(item.Groups[1].Value.Trim());
                    i++;
                }
            }
            catch (Exception ex)
            {
                Log.Error("百度电影风云榜", ex);
            }
            return Json(new { msg = list.Count > 0, data = list }, JsonRequestBehavior.DenyGet);
        }
    }
}