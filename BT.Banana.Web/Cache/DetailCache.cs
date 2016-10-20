using BT.Banana.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.Banana.Web.Cache
{
    public class DetailCache : CacheBase<ItemViewModel>
    {
        private static DetailCache cache = null;
        public static DetailCache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new DetailCache();
                }
                return cache;
            }
        }
        public DetailCache()
        {
            ExpireTime = 120;
        }
    }
}