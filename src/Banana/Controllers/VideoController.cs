using Banana.Core;
using Banana.Helper;
using Banana.Models;
using Banana.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Banana.Controllers
{
    public class VideoController : Controller
    {
        private IVideoService _mongoDbService;
        private IRedisService _redisService;
        private IMemoryCache _memoryCache;
        private IVideoRankingService _videoRankingService;

        public VideoController(IVideoService mongoDbService, IRedisService redisService, IMemoryCache memoryCache, IVideoRankingService videoRankingService)
        {
            _mongoDbService = mongoDbService;
            _redisService = redisService;
            _memoryCache = memoryCache;
            _videoRankingService = videoRankingService;
        }
        [Route("/video")]
        public IActionResult Video()
        {
            var movieListKey = "VideoIndex_MovieList";
            var movieList = new List<Video>();
            if (!_memoryCache.TryGetValue(movieListKey, out movieList))
            {
                movieList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("电影"), 1, 12);
                if (movieList != null && movieList.Count > 0)
                    _memoryCache.Set(movieListKey, movieList, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            ViewData["MovieList"] = movieList;

            var tvListKey = "VideoIndex_TVList";
            var tvList = new List<Video>();
            if (!_memoryCache.TryGetValue(tvListKey, out tvList))
            {
                tvList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("电视剧"), 1, 12);
                if (tvList != null && tvList.Count > 0)
                    _memoryCache.Set(tvListKey, tvList, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            ViewData["TVList"] = tvList;

            var varietyListKey = "VideoIndex_VarietyList";
            var varietyList = new List<Video>();
            if (!_memoryCache.TryGetValue(varietyListKey, out varietyList))
            {
                varietyList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("综艺"), 1, 12);
                if (varietyList != null && varietyList.Count > 0)
                    _memoryCache.Set(varietyListKey, varietyList, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            ViewData["VarietyList"] = varietyList;

            var animeListKey = "VideoIndex_AnimeList";
            var animeList = new List<Video>();
            if (!_memoryCache.TryGetValue(animeListKey, out animeList))
            {
                animeList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("动漫"), 1, 12);
                if (animeList != null && animeList.Count > 0)
                    _memoryCache.Set(animeListKey, animeList, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            ViewData["AnimeList"] = animeList;

            var sexListKey = "VideoIndex_SexList";
            var sexList = new List<Video>();
            if (!_memoryCache.TryGetValue(sexListKey, out sexList))
            {
                sexList = _mongoDbService.GetVideoByClassify(VideoCommonService.GetVideoClassify("伦理"), 1, 6);
                if (sexList != null && sexList.Count > 0)
                    _memoryCache.Set(sexListKey, sexList, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
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
                return Redirect("/error");
            }
            var videoDetailKey = $"VideoDetail_{id}";
            var videoDetail = _redisService.Get<VideoDetail>(videoDetailKey);
            if (videoDetail == null)
            {
                var video = _mongoDbService.GetVideo(vid);
                if (video == null)
                {
                    return Redirect("/error");
                }
                var videoSource = _mongoDbService.GetVideoSourceByVideo(video.Id);
                if (videoSource == null)
                {
                    return Redirect("/error");
                }
                videoDetail = new VideoDetail()
                {
                    Video = video,
                    VideoSource = videoSource
                };
                var cacheTime = 60;//1小时
                var type = VideoCommonService.GetVideoType(video.Classify);
                if (type == "电影" || type == "伦理")
                {
                    cacheTime = 60 * 24 * 3;//3天
                }
                _redisService.Set(videoDetailKey, videoDetail, cacheTime);
            }
            //统计
            _videoRankingService.AccessStatistics(id, videoDetail.Video.Classify);
            return View(videoDetail);
        }

        public IActionResult Play(string id, string key, string title)
        {
            ViewData["VideoPlay_Id"] = id;
            ViewData["VideoPlay_Address"] = FormatHelper.DecodeBase64(key);
            ViewData["VideoPlay_Title"] = title;
            return View();
        }
    }
}