using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Core
{
    public class CommonService
    {
        /// <summary>
        /// 总-排行榜Key
        /// </summary>
        public static string TotalRankingKey
        {
            get
            {
                return "TotalRankingKey";
            }
        }

        /// <summary>
        /// 日-电影排行榜Key
        /// </summary>
        public static string DayMovieRankingKey
        {
            get
            {
                return GetToDayString() + "DayMovieRankingKey";
            }
        }

        /// <summary>
        /// 日-电视剧排行榜Key
        /// </summary>
        public static string DayTVRankingKey
        {
            get
            {
                return GetToDayString() + "DayTVRankingKey";
            }
        }

        /// <summary>
        /// 日-综艺排行榜Key
        /// </summary>
        public static string DayVarietyRankingKey
        {
            get
            {
                return GetToDayString() + "DayVarietyRankingKey";
            }
        }

        /// <summary>
        /// 日-动漫排行榜Key
        /// </summary>
        public static string DayAnimeRankingKey
        {
            get
            {
                return GetToDayString() + "DayAnimeRankingKey";
            }
        }

        /// <summary>
        /// 日-伦理排行榜Key
        /// </summary>
        public static string DaySexRankingKey
        {
            get
            {
                return GetToDayString() + "DaySexRankingKey";
            }
        }


        public static string GetToDayString()
        {
            return DateTime.Today.ToString("yyyyMMdd");
        }
    }
}
