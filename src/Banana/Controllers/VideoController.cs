using Banana.Core;
using Banana.Helper;
using Banana.Models;
using Banana.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banana.Controllers
{
    public class VideoController : Controller
    {
        private IVideoService _videoService;
        private IRedisService _redisService;
        private IMemoryCache _memoryCache;
        private IVideoRankingService _videoRankingService;

        public VideoController(IVideoService videoService, IRedisService redisService, IMemoryCache memoryCache, IVideoRankingService videoRankingService)
        {
            _videoService = videoService;
            _redisService = redisService;
            _memoryCache = memoryCache;
            _videoRankingService = videoRankingService;
        }
        [Route("/video")]
        public IActionResult Video()
        {
            ViewData["MovieList"] = GetWeekRankingList("电影");
            ViewData["TVList"] = GetWeekRankingList("电视剧");
            ViewData["VarietyList"] = GetWeekRankingList("综艺");
            ViewData["AnimeList"] = GetWeekRankingList("动漫");
            ViewData["SexList"] = GetWeekRankingList("伦理");

            return View();
        }

        private List<VideoRank> GetWeekRankingList(string type)
        {
            var listKey = $"VideoWeekRankingList_{type}";
            var result = new List<VideoRank>();
            if (!_memoryCache.TryGetValue(listKey, out result))
            {
                result = new List<VideoRank>();
                var weekRanking = _videoRankingService.GetWeekRankingByType(type, 1, 12);//周榜前12
                var weekRankingList = _videoService.GetVideoList(weekRanking.Select(x => Convert.ToInt64(x.Key)));
                weekRanking.ForEach(item =>
                {
                    var weekRankingItem = weekRankingList.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Key));
                    if (weekRankingItem != null)
                    {
                        result.Add(ConvertVideoToVideoRank(weekRankingItem, item.Value));
                    }

                });
                if (result.Count < 12)//周榜不足12
                {
                    var count = 12 - result.Count;
                    var updateList = _videoService.GetVideoByClassify(VideoCommonService.GetVideoClassify(type), 1, count);
                    updateList.ForEach(item =>
                    {
                        result.Add(ConvertVideoToVideoRank(item, 0));
                    });
                }
                if (result != null && result.Count > 0)
                    _memoryCache.Set(listKey, result, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
            }
            return result;
        }

        private VideoRank ConvertVideoToVideoRank(Video video, double score = 0)
        {
            return new VideoRank()
            {
                Id = video.Id,
                Name = video.Name,
                Classify = video.Classify,
                Score = score,
                Image = video.Image,
                Remark = video.Remark,
                Starring = video.Starring
            };
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
            //详情
            long vid = 0;
            if (!long.TryParse(id, out vid))
            {
                return Redirect("/error");
            }
            var videoDetailKey = $"VideoDetail_{id}";
            var videoDetail = _redisService.Get<VideoDetail>(videoDetailKey);
            if (videoDetail == null)
            {
                var video = _videoService.GetVideo(vid);
                if (video == null)
                {
                    return Redirect("/error");
                }
                var videoSource = _videoService.GetVideoSourceByVideo(video.Id);
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

            var videoType = VideoCommonService.GetVideoType(videoDetail.Video.Classify); ;
            //热播
            var hotListKey = "VideoHotList_" + videoType;
            var hotList = _redisService.Get<List<VideoRank>>(hotListKey);
            if (hotList == null)
            {
                hotList = new List<VideoRank>();
                var dayRanking = _videoRankingService.GetDayRankingByType(videoType, 1, 10);//日榜前10
                var dayRankingList = _videoService.GetVideoList(dayRanking.Select(x => Convert.ToInt64(x.Key)));
                dayRanking.ForEach(item =>
                {
                    var dayRankingItem = dayRankingList.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Key));
                    if (dayRankingItem != null)
                    {
                        hotList.Add(ConvertVideoToVideoRank(dayRankingItem, item.Value));
                    }

                });
                _redisService.Set(hotListKey, hotList, 10);//10分钟
            }
            ViewData["VideoDetail_HotList"] = hotList;
            //统计
            _videoRankingService.AccessStatistics(id, videoDetail.Video.Classify);

            ViewData["VideoDetail_VideoType"] = videoType;
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