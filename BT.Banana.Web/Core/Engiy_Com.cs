using BT.Banana.Web.Helper;
using BT.Banana.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BT.Banana.Web.Core
{
    public class Engiy_Com
    {
        public static readonly string BaseUrl = "http://engiy.com/";

        public static SearchResultViewModel Search(string key, int index)
        {
            var result = new SearchResultViewModel();
            var url = BaseUrl + "s/" + key + "__1_" + index;//__1 代表搜索全部
            var html = HttpHelper.Get(url, "locale=zh-cn");//使用中文
            if (string.IsNullOrEmpty(html))
                return result;
            //搜索总数
            result.totalcount = Regex.Match(html, "<span>搜索到(.+?)个相关BT资源</span>").Groups[1].Value;
            //搜索关键字
            result.key = key;
            //搜索结果集  
            var mc = Regex.Matches(html, "<a href=\"/d/(.+?)\" target=\"_blank\">(.+?)</a>[\\s\\S]*?<span>创建于:.+?</span>");
            foreach (Match m in mc)
            {
                var item = new ItemViewModel();
                item.id = m.Groups[1].Value;
                item.name = m.Groups[2].Value;
                //result-item
                var result_item = m.Groups[0].Value;
                //files  部分没有files，需要判断
                if (Regex.IsMatch(result_item, "<div class=\"files\">"))
                {
                    var li_mc = Regex.Matches(result_item, "<li class=\"inline\">(.+?)</li>");
                    foreach (Match li in li_mc)
                    {
                        item.filedes.Add(li.Groups[1].Value);
                    }
                }
                //info
                var span_mc = Regex.Matches(result_item, "<span>.+?:(.+?)</span>");
                for (int i = 0; i < span_mc.Count; i++)
                {
                    item.type = span_mc[0].Groups[1].Value;
                    item.filecount = span_mc[1].Groups[1].Value;
                    item.filesize = span_mc[2].Groups[1].Value;
                    item.downloadcount = span_mc[3].Groups[1].Value;
                    item.updatetime = span_mc[4].Groups[1].Value;
                    item.createtime = span_mc[5].Groups[1].Value;
                }
                result.items.Add(item);
            }
            //result-pagination
            if (Regex.IsMatch(html, "<div class=\"result-pagination\">[\\s\\S]*?</div>"))
            {
                result.pagination_html = Regex.Match(html, "<div class=\"result-pagination\">[\\s\\S]*?</div>").Value;
            }
            //大家都在搜
            var a_mc = Regex.Matches(html, "href=\"/s/(.+?)___\"");
            foreach (Match a in a_mc)
            {
                result.searchwords.Add(a.Groups[1].Value);
            }
            return result;
        }


        public static ItemViewModel GetDetial(string id)
        {
            var result = new ItemViewModel();
            var url = BaseUrl + "d/" + id;
            var html = HttpHelper.Get(url, "locale=zh-cn");//使用中文
            if (string.IsNullOrEmpty(html))
                return result;
            return result;
        }


    }
}