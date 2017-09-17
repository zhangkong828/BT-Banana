using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Banana.Web.Models;
using Microsoft.Extensions.Localization;
using Banana.Common;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Banana.Web.Services;

namespace Banana.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRedisService _redisService;
        private readonly IElasticSearchService _elasticSearchService;

        public HomeController()
        {
            //_redisService = redisService;
            //_elasticSearchService = elasticSearchService;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 百度电影风云榜
        /// </summary>
        [HttpPost]
        public IActionResult GetBaiDuHotKeys([FromForm]int num)
        {
            List<string> list = new List<string>();
            try
            {
                string url = "http://top.baidu.com/buzz?b=26&c=1&fr=topcategory_c1";
                var html = HttpHelper.Get(url, null, "gb2312");
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
                //Log.Error("百度电影风云榜", ex);
            }
            return Json(new { msg = list.Count > 0, data = list });
        }


        /// <summary>
        /// 磁力搜索
        /// </summary>
        [Route("/s/magnet/{key}/{index?}")]
        public IActionResult Search(string key, string index)
        {
            if (string.IsNullOrEmpty(key))
                return RedirectToAction("index");
            key = key.Trim();
            var currentIndex = 0;
            if (!int.TryParse(index, out currentIndex))
                currentIndex = 1;
            currentIndex = currentIndex < 1 ? 1 : currentIndex;

            return View();
        }

        /// <summary>
        /// 详情页
        /// </summary>
        public IActionResult D(string id)
        {
            return View();
        }
    }
}
