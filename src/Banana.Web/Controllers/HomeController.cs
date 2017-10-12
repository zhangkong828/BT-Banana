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


        /// <summary>
        /// 首页
        /// </summary>
        public IActionResult Index()
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


        public IActionResult About()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }



        /// <summary>
        /// 视频解析
        /// </summary>
        [Route("/analyse/{url?}")]
        public IActionResult Analyse(string url)
        {
            ViewData["gkey"] = Guid.NewGuid().ToString("n");
            return View();
        }

        [HttpPost]
        [Route("/analyse/getkey")]
        public IActionResult AnalyseKey(string url, string gkey)
        {
            var now = DateTime.Now;
            var time = now.ToString("yyyy-MM-dd HH:mm:ss");
            var timestamp = FormatHelper.ConvertToTimeStamp(now);
            return View();
        }

        [Route("/analyse/frame")]
        public IActionResult AnalyseFrame(string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                return NotFound();
            }
            ViewData["url"] = url;
            ViewData["key"] = key;
            return View();
        }

        [Route("/analyse/core")]
        public ContentResult AnalyseCore(string k, string u, int type)
        {
            if (string.IsNullOrEmpty(k) || string.IsNullOrEmpty(u) || type > 1 || type < 0)
            {
                return Content("参数错误！");
            }
            var token = k.Trim();
            var url = u.Trim();
            var result = new List<string>();
            var list = new List<string>()
            {
                "http://movie.ks.js.cn/flv/other/1_0.mp4"
            };
            foreach (var item in list)
            {
                if (type == 1)
                {
                    result.Add($"{item}->video/mp4");
                }
                else
                {
                    result.Add(item);
                }

            }
            return Content(string.Join("|", result), "text/plain");
        }


        /// <summary>
        /// 磁力链接 搜索
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
        /// 磁力链接 详情页
        /// </summary>
        [Route("/d/magnet/{hash}")]
        public IActionResult Detail(string hash)
        {
            return View();
        }
    }
}
