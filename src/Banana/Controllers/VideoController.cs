using Banana.Core;
using Banana.Services;
using Banana.Services.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace Banana.Controllers
{
    public class VideoController : Controller
    {
        private IMongoDbService _mongoDbService;
        private IRedisService _redisService;
        public VideoController(IMongoDbService mongoDbService, IRedisService redisService)
        {
            _mongoDbService = mongoDbService;
            _redisService = redisService;
        }
        [Route("/video")]
        public IActionResult Video()
        {
            ViewData["MovieList"] = _mongoDbService.GetVideoByClassify(CommonService.GetVideoClassify("电影"), 1, 12);
            ViewData["TVList"] = _mongoDbService.GetVideoByClassify(CommonService.GetVideoClassify("电视剧"), 1, 12);
            ViewData["VarietyList"] = _mongoDbService.GetVideoByClassify(CommonService.GetVideoClassify("综艺"), 1, 12);
            ViewData["AnimeList"] = _mongoDbService.GetVideoByClassify(CommonService.GetVideoClassify("动漫"), 1, 12);
            ViewData["SexList"] = _mongoDbService.GetVideoByClassify(CommonService.GetVideoClassify("伦理"), 1, 6);
            return View();
        }


        [Route("/s/video/{key}/{index?}")]
        public IActionResult VideoSearch(string key, string index)
        {
            //key = key.Trim();
            //if (string.IsNullOrEmpty(key))
            //    return RedirectToAction("index");
            //int.TryParse(index, out int currentIndex);
            //currentIndex = currentIndex < 1 ? 1 : currentIndex > 100 ? 100 : currentIndex; //最多100页
            //var pageSize = 10;
            //var result = new MagnetLinkSearchResultViewModel();
            ////只缓存前20页数据  1分钟
            //if (currentIndex > 20)
            //{
            //    result = _elasticSearchService.MagnetLinkSearch(key, currentIndex, pageSize);
            //}
            //else
            //{
            //    var cacheKey = $"s_m_{key}_{currentIndex}";
            //    if (!_memoryCache.TryGetValue(cacheKey, out result))
            //    {
            //        result = _elasticSearchService.MagnetLinkSearch(key, currentIndex, pageSize);
            //        _memoryCache.Set(cacheKey, result, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
            //    }
            //}
            return View();
        }

        [Route("/d/video/{id}")]
        public IActionResult VideoDetail(string id)
        {
            long vid = 0;
            if (!long.TryParse(id, out vid))
            {
                //return "404";
            }
            var video = _mongoDbService.GetVideo(vid);
            if (video == null)
            {
                //return "404";
            }
            return View(video);
        }
    }
}