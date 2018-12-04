using Banana.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// <param name="classify"></param>
        /// <returns></returns>
        private string GetDayRankingKey(string classify, DateTime? date = null)
        {
            var type = VideoCommonService.GetVideoType(classify) ?? "";
            return $"{VideoCommonService.DayRankingKey}{(date.HasValue ? date.Value.ToString("yyyyMMdd") : DateTime.Today.ToString("yyyyMMdd"))}{type}";
        }

        /// <summary>
        /// 本周排行key
        /// </summary>
        private string GetCurrentWeekRankingKey(string classify)
        {
            var type = VideoCommonService.GetVideoType(classify) ?? "";
            var start = Utility.GetWeekUpOfDate(DateTime.Now, DayOfWeek.Monday, 0).Date;
            var end = Utility.GetWeekUpOfDate(DateTime.Now, DayOfWeek.Sunday, 1).Date;
            return $"{VideoCommonService.WeekRankingKey}{start.ToString("yyyyMMdd")}{end.ToString("yyyyMMdd")}{type}";
        }


        public bool AccessStatistics(string id, string classify)
        {
            //日排行
            _redisService.SortedSetIncrement(GetDayRankingKey(classify), id, 1, 60 * 24 * 15);//15天
            //总排行
            _redisService.SortedSetIncrement(VideoCommonService.TotalRankingKey, id, 1);
            return true;
        }

        public List<KeyValuePair<string, double>> GetDayRanking(string classify, int pageindex, int pagesize)
        {
            return _redisService.SortedSetRangeByRankWithScores(GetDayRankingKey(classify), pageindex, pagesize);
        }

        public List<KeyValuePair<string, double>> GetWeekRanking(string classify, int pageindex, int pagesize)
        {
            return _redisService.SortedSetRangeByRankWithScores(GetCurrentWeekRankingKey(classify), pageindex, pagesize);
        }
    }
}
