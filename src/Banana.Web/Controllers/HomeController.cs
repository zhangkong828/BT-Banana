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
            return View();
        }

        [Route("/analyse/frame")]
        public IActionResult AnalyseFrame(string url)
        {
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
                "http://111.202.85.147/vipzj.video.tc.qq.com/ACN8B7d9nhTRUYStQHhcRjZP1TTNjWUlDXEIWbEhpY8A/x0012ezj2z6.p201.1.mp4?vkey=4EE9EB8EA76C44ECB6A3B0302C56D204A0E825DB7B66ED0D4BB6D71E1F8F9AF1E09BD89A0567511E8B076A3FFA88D3EC69206DBB6AD1AEF8523E856B620B86610D60B695F217342423828A4E5D31164DC1139E5574F6C4295895C9E0483EEB85",
                "http://111.202.85.147/vipzj.video.tc.qq.com/ACN8B7d9nhTRUYStQHhcRjZP1TTNjWUlDXEIWbEhpY8A/x0012ezj2z6.p201.2.mp4?vkey=B104F6A614C14FCE56326FD45BC1D367D45D7009BFD2499DDF3EE7BD24D04EF637ED1A85B91D9C02FF9D61FB8253846100A0BD228E67C8BEE2884071E6E086D12B60CD8AD6FFBFBA6A87E6B2EBAD23C8E7955C5A8BF21970B6C471F84EB36338"
            };
            foreach (var item in list)
            {
                if (type == 1)
                {
                    //result = "http://movie.ks.js.cn/flv/other/1_0.mp4->video/mp4";
                    result.Add($"{item}->video/mp4");
                }
                else
                {
                    //result = "http://movie.ks.js.cn/flv/other/1_0.mp4";
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
