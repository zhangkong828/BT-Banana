using System;
using System.Collections.Generic;

namespace Banana.Core
{
    public class CommonService
    {
        public static List<string> GetVideoClassify(string type)
        {
            switch (type)
            {
                case "电影":
                    return new List<string>() { "动作片", "喜剧片", "爱情片", "科幻片", "恐怖片", "剧情片", "战争片", "纪录片" };
                case "电视剧":
                    return new List<string>() { "国产剧", "港剧", "日剧", "欧美剧", "韩剧", "台剧", "泰剧", "越南剧" };
                case "综艺":
                    return new List<string>() { "综艺" };
                case "动漫":
                    return new List<string>() { "动漫" };
                case "伦理":
                    return new List<string>() { "伦理片", "写真视频", "美女写真", "美女视频", "	韩国主播VIP视频" };
                default:
                    return null;
            }
        }

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
