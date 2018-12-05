using Banana.Core;
using Banana.Models;
using Banana.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Banana.Controllers
{
    public class VideoController : Controller
    {
        private IVideoService _mongoDbService;
        private IRedisService _redisService;
        public VideoController(IVideoService mongoDbService, IRedisService redisService)
        {
            _mongoDbService = mongoDbService;
            _redisService = redisService;
        }
        [Route("/video")]
        public IActionResult Video()
        {
            var movieListKey = "VideoIndex_MovieList";
            var movieList = _redisService.Get<List<Video>>(movieListKey);
            if (movieList == null)
            {
                movieList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("电影"), 1, 12);
                _redisService.Set(movieListKey, movieList, 10);
            }
            ViewData["MovieList"] = movieList;

            var tvListKey = "VideoIndex_TVList";
            var tvList = _redisService.Get<List<Video>>(tvListKey);
            if (tvList == null)
            {
                tvList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("电视剧"), 1, 12);
                _redisService.Set(tvListKey, tvList, 10);
            }
            ViewData["TVList"] = tvList;

            var varietyListKey = "VideoIndex_VarietyList";
            var varietyList = _redisService.Get<List<Video>>(varietyListKey);
            if (varietyList == null)
            {
                varietyList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("综艺"), 1, 12);
                _redisService.Set(varietyListKey, varietyList, 10);
            }
            ViewData["VarietyList"] = varietyList;

            var animeListKey = "VideoIndex_AnimeList";
            var animeList = _redisService.Get<List<Video>>(animeListKey);
            if (animeList == null)
            {
                animeList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("动漫"), 1, 12);
                _redisService.Set(animeListKey, animeList, 10);
            }
            ViewData["AnimeList"] = animeList;

            var sexListKey = "VideoIndex_SexList";
            var sexList = _redisService.Get<List<Video>>(sexListKey);
            if (sexList == null)
            {
                sexList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("伦理"), 1, 6);
                _redisService.Set(sexListKey, sexList, 10);
            }
            ViewData["SexList"] = sexList;

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
            var videoSource = _mongoDbService.GetVideoSourceByVideo(video.Id);
            ViewData["VideoSource"] = videoSource;
            return View(video);
        }
    }
}