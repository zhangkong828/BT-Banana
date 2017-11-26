using Banana.Web.Helper;
using Banana.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Banana.Web.Core
{
    public class SearchService
    {
        public static SearchResultViewModel QQSearch(string key, int pageindex = 1)
        {
            var result = new SearchResultViewModel()
            {
                SearchKey = key
            };

            var searchUrl = $"https://v.qq.com/x/search/?q={key}&cur={pageindex}";
            var html = HttpHelper.Get(searchUrl);
            if (string.IsNullOrEmpty(html))
                return result;
            //匹配搜索结果中的a标签
            var a_Matchs = Regex.Matches(html, "<a.+?_stat=\"video:poster_tle\".+?</a>|<a.+?_stat=\"video:poster_h_title\".+?</a>");
            if (a_Matchs.Count <= 0)
                return result;
            foreach (Match match in a_Matchs)
            {
                var input = match.Value;
                //获取url title
                var regex = new Regex("href=\"(.+?)\".+?>(.+?)</a>");
                if (!regex.IsMatch(input))
                    continue;
                var url = regex.Match(input).Groups[1].Value;
                //过滤非v.qq的地址，以及合集、连续剧
                if (string.IsNullOrEmpty(url) || (!url.Contains("v.qq.com/x/cover") && !url.Contains("v.qq.com/x/page")))
                    continue;
                var title = regex.Match(input).Groups[2].Value;

                result.SearchResult.Add(new TencentItem()
                {
                    Url = url,
                    Name = title
                });

            }
            //匹配分页结果， 结果集有过滤，每页的数据量不会相同 所以只取最大页
            var search_container = Regex.Match(html, "class=\"search_container\"[\\s\\S]*?>").Value;
            if (string.IsNullOrEmpty(search_container))
                return result;
            var pages = Regex.Match(search_container, "pages:(.+?);").Groups[1].Value.Trim();
            var cur = Regex.Match(search_container, "cur:(.+?);").Groups[1].Value.Trim();

            int.TryParse(pages, out int totals);
            int.TryParse(cur, out pageindex);
            result.PageIndex = pageindex;
            result.PageTotals = totals;
            return result;
        }
    }
}
