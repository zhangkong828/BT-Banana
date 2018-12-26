using CSRedis;
using System.Collections.Generic;

namespace Banana.Services
{
    public class RedisService : IRedisService
    {
        private readonly CSRedisClient _client;

        public RedisService(CSRedisClient client)
        {
            _client = client;
        }

        public string Get(string key)
        {
            return _client.Get(key);
        }

        public T Get<T>(string key)
        {
            return _client.Get<T>(key);
        }

        public bool IsExist(string key)
        {
            return _client.Exists(key);
        }

        public bool Set(string key, object data, int cacheTime = 0)
        {
            var expiry = -1;
            if (cacheTime > 0)
                expiry = cacheTime * 60;
            return _client.Set(key, data, expiry);
        }

        public bool SortedSetIncrement(string key, string member, double score, int cacheTime = 0)
        {
            _client.ZIncrBy(key, member, score);
            if (cacheTime > 0)
            {
                var expiry = cacheTime * 60;
                return _client.Expire(key, expiry);
            }
            return true;
        }

        public double? SortedSetScore(string key, string member)
        {
            return _client.ZScore(key, member);
        }

        public List<KeyValuePair<string, double>> SortedSetRangeByRankWithScores(string key, int pageindex, int pagesize)
        {
            var range = _client.ZRevRangeWithScores(key, (pageindex - 1) * pagesize, pageindex * pagesize - 1);
            var result = new List<KeyValuePair<string, double>>();
            foreach (var item in range)
            {
                result.Add(new KeyValuePair<string, double>(item.member, item.score));
            }
            return result;
        }

        public long SortedSetCombineAndStore(string destinationKey, List<string> keys, int cacheTime = 0)
        {
            var num = _client.ZUnionStore(destinationKey, null, RedisAggregate.Sum, keys.ToArray());
            if (cacheTime > 0)
            {
                var expiry = cacheTime * 60;
                _client.Expire(destinationKey, expiry);
            }
            return num;
        }
    }
}
