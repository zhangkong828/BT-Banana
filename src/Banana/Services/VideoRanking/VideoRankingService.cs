using Banana.Core;
using System;
using System.Collections.Generic;

namespace Banana.Services
{
    public class VideoRankingService : IVideoRankingService
    {
        private IRedisService _redisService;
        public VideoRankingService(IRedisService redisService)
        {
            _redisService = redisService;
        }

        /// <summary>
        /// 日排行key
        /// </summary>
        private string GetDayRankingKeyByType(string type, DateTime? date = null)
        {
            return $"{VideoCommonService.DayRankingKey}{(date.HasValue ? date.Value.ToString("yyyyMMdd") : DateTime.Today.ToString("yyyyMMdd"))}{type}";
        }

        /// <summary>
        /// 本周排行key
        /// </summary>
        private string GetCurrentWeekRankingKeyByType(string type, out DateTime start, out DateTime end)
        {
            start = Utility.GetWeekUpOfDate(DateTime.Now, DayOfWeek.Monday, 0).Date;
            end = Utility.GetWeekUpOfDate(DateTime.Now, DayOfWeek.Sunday, 1).Date;
            return $"{VideoCommonService.WeekRankingKey}{start.ToString("yyyyMMdd")}{end.ToString("yyyyMMdd")}{type}";
        }

        public bool AccessStatistics(string id, string classify)
        {
            var type = VideoCommonService.GetVideoType(classify) ?? "";
            //日排行
            _redisService.SortedSetIncrement(GetDayRankingKeyByType(type), id, 1, 60 * 24 * 15);//15天
            //总排行
            _redisService.SortedSetIncrement(VideoCommonService.TotalRankingKey, id, 1);
            return true;
        }


        public List<KeyValuePair<string, double>> GetDayRankingByType(string type, int pageindex, int pagesize)
        {
            if (pageindex <= 1)
                pageindex = 1;
            return _redisService.SortedSetRangeByRankWithScores(GetDayRankingKeyByType(type), pageindex, pagesize);
        }


        public List<KeyValuePair<string, double>> GetWeekRankingByType(string type, int pageindex, int pagesize)
        {
            if (pageindex <= 1)
                pageindex = 1;
            lock (type)
            {
                //本周排行key
                var currentWeekRankingKey = GetCurrentWeekRankingKeyByType(type, out DateTime start, out DateTime end);
                if (!_redisService.IsExist(currentWeekRankingKey))
                {
                    //获取本周的日key
                    var dayKeys = new List<string>();
                    while (DateTime.Compare(start, end) <= 0)
                    {
                        dayKeys.Add(GetDayRankingKeyByType(type, start));
                        start = start.AddDays(1);
                    }
                    //合并日key 计算到周key
                    _redisService.SortedSetCombineAndStore(currentWeekRankingKey, dayKeys, 60);//1小时
                }
                return _redisService.SortedSetRangeByRankWithScores(currentWeekRankingKey, pageindex, pagesize);
            }
        }

        public int GetAccessCount(string id, string classify)
        {
            var num = _redisService.SortedSetScore(VideoCommonService.TotalRankingKey, id);
            return num.HasValue ? (int)num.Value : 0;
        }

        public List<KeyValuePair<string, double>> GetTotalRanking(int pageindex, int pagesize)
        {
            if (pageindex <= 1)
                pageindex = 1;
            return _redisService.SortedSetRangeByRankWithScores(VideoCommonService.TotalRankingKey, pageindex, pagesize);
        }
    }
}
