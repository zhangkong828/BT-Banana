
using BT.Banana.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.Banana.Web.Cache
{
    public class SearchCache: CacheBase<SearchResultViewModel>
    {
        private static SearchCache cache = null;
        public static SearchCache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new SearchCache();
                }
                return cache;
            }
        }
        public SearchCache()
        {
            ExpireTime = 10;
        }


    }
}