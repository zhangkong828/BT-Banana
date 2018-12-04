using System;
using System.Collections.Generic;

namespace Banana.Core
{
    public class VideoCommonService
    {
        public static Dictionary<string, List<string>> _classifyDic;
        static VideoCommonService()
        {
            _classifyDic.Add("电影", new List<string>() { "动作片", "喜剧片", "爱情片", "科幻片", "恐怖片", "剧情片", "战争片", "纪录片" });
            _classifyDic.Add("电视剧", new List<string>() { "国产剧", "港剧", "日剧", "欧美剧", "韩剧", "台剧", "泰剧", "越南剧" });
            _classifyDic.Add("综艺", new List<string>() { "综艺" });
            _classifyDic.Add("动漫", new List<string>() { "动漫" });
            _classifyDic.Add("伦理", new List<string>() { "伦理片", "写真视频", "美女写真", "美女视频", "韩国主播VIP视频" });
        }

        public static List<string> GetVideoClassify(string type)
        {
            if (_classifyDic.ContainsKey(type))
                return _classifyDic[type];
            else
                return null;
        }

        public static string GetVideoType(string classify)
        {
            foreach (var item in _classifyDic)
            {
                if (item.Value.Contains(classify))
                    return item.Key;
            }
            return null;
        }

        /// <summary>
        /// 总排行榜Key
        /// </summary>
        public static string TotalRankingKey
        {
            get
            {
                return "TotalRankingKey";
            }
        }

        /// <summary>
        /// 周排行榜Key
        /// </summary>
        public static string WeekRankingKey
        {
            get
            {
                return "WeekRankingKey";
            }
        }

        /// <summary>
        /// 日排行榜Key
        /// </summary>
        public static string DayRankingKey
        {
            get
            {
                return "DayRankingKey";
            }
        }



    }
}
