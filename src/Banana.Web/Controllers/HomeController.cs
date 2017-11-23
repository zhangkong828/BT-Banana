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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;

namespace Banana.Web.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache _memoryCache;
        private readonly IRedisService _redisService;
        private readonly IElasticSearchService _elasticSearchService;

        public HomeController(IMemoryCache memoryCache, IRedisService redisService, IElasticSearchService elasticSearchService)
        {
            _memoryCache = memoryCache;
            _redisService = redisService;
            _elasticSearchService = elasticSearchService;
        }


        /// <summary>
        /// pc首页
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        public IActionResult MobileIndex()
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

        [Route("/error")]
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
            var gkey = HttpContext.Session.GetString("gkey");
            if (string.IsNullOrEmpty(gkey))
            {
                gkey = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("gkey", gkey);
            }
            ViewData["gkey"] = gkey;
            return View();
        }

        [HttpPost]
        [Route("/analyse/getkey")]
        public IActionResult AnalyseKey([FromForm]string url, [FromForm]string gkey)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(gkey))
            {
                return Json(new { errorCode = -1, msg = "参数不完整，缺少相关参数" });
            }
            var now = DateTime.Now;
            var time = now.ToString("yyyy-MM-dd HH:mm:ss");
            var timestamp = FormatHelper.ConvertToTimeStamp(now).ToString();
            var sign = CreateSign(url, gkey, timestamp);
            return Json(new { errorCode = 0, url = url, gkey = gkey, timestamp = timestamp, sign = sign });
        }

        [Route("/analyse/frame")]
        public IActionResult AnalyseFrame([FromQuery]string url, [FromQuery] string gkey, [FromQuery]string timestamp, [FromQuery]string sign)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(gkey) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(sign))
            {
                return NotFound();
            }
            ViewData["url"] = FormatHelper.UrlEncode(FormatHelper.UrlDecode(url));
            ViewData["gkey"] = gkey;
            ViewData["timestamp"] = timestamp;
            ViewData["sign"] = sign;
            return View();
        }

        [HttpPost]
        [Route("/analyse/core")]
        public IActionResult AnalyseCore([FromForm]string u, [FromForm]string g, [FromForm]string t, [FromForm]string s)
        {
            var url = u.Trim();
            var gkey = g;
            var timestamp = t;
            var sign = s;
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(gkey) | string.IsNullOrEmpty(timestamp) | string.IsNullOrEmpty(sign))
            {
                return Json(new { errorCode = -9999, msg = "参数错误，请求非法！" });
            }
            long timestampl = 0;
            long.TryParse(timestamp, out timestampl);
            var time = FormatHelper.ConvertToDateTime(timestampl);
            //不得超过2分钟
            if (DateTime.Compare(time.AddMinutes(2), DateTime.Now) < 0)
            {
                return Json(new { errorCode = -10000, msg = "请求已过期" });
            }
            //gkey需要与会话一致
            var server_gkey = HttpContext.Session.GetString("gkey");
            if (gkey != server_gkey)
            {
                return Json(new { errorCode = -10001, msg = "会话超时，请求非法！" });
            }
            //校验签名
            if (!ValidateSign(url, gkey, timestamp, sign))
            {
                return Json(new { errorCode = -10002, msg = "签名错误，请求非法！" });
            }
            //url解码
            url = FormatHelper.UrlDecode(url);
            var result = new List<string>();
            var list = new List<string>()
            {
                "http://movie.ks.js.cn/flv/other/1_0.mp4"
            };
            foreach (var item in list)
            {
                //result.Add($"{item}->video/mp4");
                result.Add(item);

            }
            return Json(new { errorCode = 0, urls = string.Join("|", result) });
        }


        private string CreateSign(string url, string gkey, string timestamp)
        {
            var MD5Key = "btbanana.com";
            return FormatHelper.Md5Hash($"md5key={MD5Key}&url={url}&gkey={gkey}&timestamp={timestamp}&md5key={MD5Key}");
        }

        private bool ValidateSign(string url, string gkey, string timestamp, string sign)
        {
            var MD5Key = "btbanana.com";
            var sourceSign = FormatHelper.Md5Hash($"md5key={MD5Key}&url={url}&gkey={gkey}&timestamp={timestamp}&md5key={MD5Key}");
            return sourceSign == sign;
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
            int.TryParse(index, out currentIndex);
            currentIndex = currentIndex < 1 ? 1 : currentIndex;
            var pageSize = 10;
            var result = _elasticSearchService.MagnetLinkSearch(key, currentIndex, pageSize);
            return View(result);
        }

        /// <summary>
        /// 磁力链接 详情页
        /// </summary>
        [Route("/d/magnet/{hash}")]
        public IActionResult Detail(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 40)
                return new RedirectResult("/error");
            var model = _elasticSearchService.MagnetLinkInfo(hash);
            if (model == null)
                return new RedirectResult("/error");
            return View(model);
        }
    }
}
