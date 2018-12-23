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
            throw new Exception("11");
            ViewData["MovieList"] = GetUpdateList("电影");
            ViewData["TVList"] = GetUpdateList("电视剧");
            ViewData["VarietyList"] = GetUpdateList("综艺");
            ViewData["AnimeList"] = GetUpdateList("动漫");
            ViewData["SexList"] = GetUpdateList("伦理");

            return View();
        }

        private List<Video> GetUpdateList(string type)
        {
            int count = type == "伦理" ? 6 : 12;
            var listKey = $"VideoIndexUpdateList_{type}";
            if (!_memoryCache.TryGetValue(listKey, out List<Video> result))
            {
                result = new List<Video>();
                result = _videoService.GetVideoByClassify(VideoCommonService.GetVideoClassify(type), 1, count);
                if (result != null && result.Count > 0)
                    _memoryCache.Set(listKey, result, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
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
            key = key.Trim();
            if (string.IsNullOrEmpty(key))
                return Redirect("/video");
            int.TryParse(index, out int currentIndex);
            currentIndex = currentIndex < 1 ? 1 : currentIndex;
            var pageSize = 10;

            var searchKey = $"VideoSearch_{key}_{currentIndex}";
            var searchResult = _redisService.Get<VideoSearchResult>(searchKey);
            if (searchResult == null)
            {
                long totalCount = 0;
                searchResult = new VideoSearchResult() { PageIndex = currentIndex, PageSize = pageSize };
                var result = _videoService.SearchVideo(key, currentIndex, pageSize, out totalCount);
                searchResult.Result = result;
                searchResult.TotalCount = totalCount;
                searchResult.Key = key;
                if (result != null && result.Count > 0)
                    _redisService.Set(searchKey, searchResult, 10);
            }
            ViewData["VideoSearchResult"] = searchResult;

            //总榜
            var totalRankingKey = "VideoSearch_TotalRankingKey";
            if (!_memoryCache.TryGetValue(totalRankingKey, out List<VideoRank> totalRankingList))
            {
                totalRankingList = new List<VideoRank>();
                var totalRanking = _videoRankingService.GetTotalRanking(1, 20);
                var rankingList = _videoService.GetVideoList(totalRanking.Select(x => Convert.ToInt64(x.Key)));
                totalRanking.ForEach(item =>
                {
                    var totalRankingItem = rankingList.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Key));
                    if (totalRankingItem != null)
                    {
                        totalRankingList.Add(ConvertVideoToVideoRank(totalRankingItem, item.Value));
                    }

                });
                _memoryCache.Set(totalRankingKey, totalRankingList, new DateTimeOffset(DateTime.Now.AddHours(1)));//1小时
            }
            ViewData["VideoSearch_TotalRanking"] = totalRankingList;
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
            if (!_memoryCache.TryGetValue(hotListKey, out List<VideoRank> hotList))
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
                _memoryCache.Set(hotListKey, hotList, new DateTimeOffset(DateTime.Now.AddMinutes(5)));//5分钟
            }
            ViewData["VideoDetail_HotList"] = hotList;
            //推荐
            var recommendKey = "VideoRecommendList_" + videoType;
            if (!_memoryCache.TryGetValue(recommendKey, out List<VideoRank> recommendList))
            {
                recommendList = new List<VideoRank>();
                var weekRanking = _videoRankingService.GetWeekRankingByType(videoType, 1, 4);//周榜前4
                var weekRankingList = _videoService.GetVideoList(weekRanking.Select(x => Convert.ToInt64(x.Key)));
                weekRanking.ForEach(item =>
                {
                    var weekRankingItem = weekRankingList.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Key));
                    if (weekRankingItem != null)
                    {
                        recommendList.Add(ConvertVideoToVideoRank(weekRankingItem, item.Value));
                    }

                });
                _memoryCache.Set(recommendKey, recommendList, new DateTimeOffset(DateTime.Now.AddHours(1)));//1小时
            }
            ViewData["VideoDetail_RecommendList"] = recommendList;
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