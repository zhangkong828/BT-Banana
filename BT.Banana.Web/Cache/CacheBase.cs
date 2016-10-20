using BT.Banana.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BT.Banana.Web.Cache
{
    public class CacheBase<T>
    {
        protected static object synObj = new object();
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        public Dictionary<string, Tuple<T, int>> innerData = null;
        public int ThresholdCount = 200;//最大阈值 超过该值 则自动清理过期cache
        public int ExpireTime = 5;//过期时间 单位：分钟
        public CacheBase()
        {
            innerData = new Dictionary<string, Tuple<T, int>>();
        }
        public void Set(string key, T val)
        {
            rwLock.EnterWriteLock();
            try
            {
                var expire = FormatHelper.ConvertDateTimeInt(DateTime.Now.AddMinutes(ExpireTime));
                if (innerData.ContainsKey(key))
                {
                    innerData[key] = new Tuple<T, int>(val, expire);
                }
                else
                {
                    if (innerData.Count >= ThresholdCount)
                        Clear();
                    innerData.Add(key, new Tuple<T, int>(val, expire));
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public T Get(string key)
        {
            T result = default(T);
            Tuple<T, int> tuple = default(Tuple<T, int>);
            try
            {
                rwLock.EnterReadLock();
                if (innerData.TryGetValue(key, out tuple))
                {
                    var expire = tuple.Item2;
                    var nowtime = FormatHelper.ConvertDateTimeInt(DateTime.Now);
                    if (expire < nowtime)
                    {
                        //过期删除
                        innerData.Remove(key);
                    }
                    else
                        result = tuple.Item1;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
            return result;
        }

        public void Clear()
        {
            var keys = new List<string>(innerData.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var tuple = innerData[keys[i]];
                var expire = tuple.Item2;
                var nowtime = FormatHelper.ConvertDateTimeInt(DateTime.Now);
                if (expire < nowtime)
                {
                    //过期删除
                    innerData.Remove(keys[i]);
                }
            }
        }
    }
}