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
        private static readonly string BaseUrl = "http://engiy.com/";

        public static SearchResultViewModel Search(string key, int index,string cultureName)
        {
            var result = new SearchResultViewModel();
            var url = BaseUrl + "s/" + key + "__1_" + index;//__1 代表搜索全部
            cultureName=cultureName == "zh-CN" ? "zh-cn" : "en";
            var cookie = $"locale={cultureName}";
            var html = HttpHelper.Get(url, cookie);
            if (string.IsNullOrEmpty(html))
                return result;
            //多语言下 正则 注意区分中英文
            var totalcount_regex = cultureName == "zh-cn"? "<span>搜索到(.+?)个相关BT资源</span>": "<span>About (.+?) Bittorrent results</span>";
            var body_regex = cultureName == "zh-cn" ? "<a href=\"/d/(.+?)\" target=\"_blank\">(.+?)</a>[\\s\\S]*?<span>创建于:.+?</span>" : "<a href=\"/d/(.+?)\" target=\"_blank\">(.+?)</a>[\\s\\S]*?<span>Created:.+?</span>";
            //搜索总数
            result.totalcount = Regex.Match(html, totalcount_regex).Groups[1].Value;
            //搜索关键字
            result.key = key;
            //搜索结果集  
            var mc = Regex.Matches(html, body_regex);
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
                item.type = span_mc[0].Groups[1].Value;
                item.filecount = span_mc[1].Groups[1].Value;
                item.filesize = span_mc[2].Groups[1].Value;
                item.downloadcount = span_mc[3].Groups[1].Value;
                item.updatetime = span_mc[4].Groups[1].Value;
                item.createtime = span_mc[5].Groups[1].Value;
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


        public static ItemViewModel GetDetial(string id,string cultureName)
        {
            var item = new ItemViewModel();
            var url = BaseUrl + "d/" + id;
            cultureName = cultureName == "zh-CN" ? "zh-cn" : "en";
            var cookie = $"locale={cultureName}";
            var html = HttpHelper.Get(url, cookie);
            if (string.IsNullOrEmpty(html))
                return item;
            //id
            item.id = Regex.Match(html, "<h4 class=\"inline\"><a href=\"/d/(.+?)\">.+?</a></h4>").Groups[1].Value;
            //名称
            item.name = Regex.Match(html, "<h4 class=\"inline\"><a href=\"/d/.+?\">(.+?)</a></h4>").Groups[1].Value;
            //磁力链接
            item.magnet = Regex.Match(html, "<a href=\"magnet.+?>(.+?)<").Groups[1].Value;
            //info
            var td_mc = Regex.Matches(html, "<td>(.+?)</td>");
            item.type = td_mc[0].Groups[1].Value;
            item.filesize = td_mc[1].Groups[1].Value;
            item.filecount = td_mc[2].Groups[1].Value;
            item.downloadcount = td_mc[3].Groups[1].Value;
            item.updatetime = td_mc[4].Groups[1].Value;
            item.createtime = td_mc[5].Groups[1].Value;
            //搜索关键字
            var a_mc = Regex.Matches(html, "<a href=\"/s/.+?\" class=\"label label-default\">(.+?)</a>");
            foreach (Match a in a_mc)
            {
                item.searchkeywords.Add(a.Groups[1].Value);
            }
            //文件列表
            var li_mc = Regex.Matches(html, "<li class=\"inline\">(.+?)\\((.+?)\\)</li>");
            foreach (Match li in li_mc)
            {
                item.filelist.Add(new FileInfo()
                {
                    name = li.Groups[1].Value,
                    size = li.Groups[2].Value
                });
            }
            //你可能也喜欢
            var div_mc = Regex.Matches(html, "<div style=\"margin:6px 0px\" class=\"inline\">[\\s\\S]*?<a href=\"/d/(.+?)\">(.+?)</a>");
            foreach (Match div in div_mc)
            {
                item.soyoulike.Add(new ItemInfo()
                {
                    id = div.Groups[1].Value,
                    name = div.Groups[2].Value
                });
            }
            return item;
        }


    }
}