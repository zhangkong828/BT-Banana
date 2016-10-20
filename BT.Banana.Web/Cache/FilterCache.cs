using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BT.Banana.Web.Cache
{
    public class FilterCache
    {
        private static object obj = new object();
        private static FilterCache _cache;
        public static FilterCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    lock (obj)
                    {
                        if (_cache == null)
                        {
                            _cache = new FilterCache();
                        }
                    }
                }
                return _cache;
            }
        }

        private static List<string> dic;
        public FilterCache()
        {
            dic = new List<string>();
        }


        private void Init()
        {
            var path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "filter.txt");
            var lines = File.ReadAllLines(path);
            foreach (var item in lines)
            {
                var hash = Regex.Match(item.ToLower(), "d/(.+)\\.html").Groups[1].Value;
                if (!string.IsNullOrEmpty(hash) && !dic.Contains(hash))
                {
                    dic.Add(hash.ToLower());
                }
            }
        }


        public bool IsContains(string hash)
        {
            var result = false;
            if (dic == null || dic.Count <= 0)
            {
                Init();
            }
            if (dic.Contains(hash.ToLower()))
            {
                result = true;
            }
            return result;
        }


    }
}