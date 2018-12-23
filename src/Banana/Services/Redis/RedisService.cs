using Banana.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Banana.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _configInfos;
        private readonly IDatabase _database;

        private readonly string DefaultKey;
        public RedisService(IConfiguration config)
        {
            _configInfos = config;
            _database = RedisHelper.GetDateBase();

            DefaultKey = _configInfos["Redis:DefaultKey"];
        }

        #region Private

        private string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }

        private string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private T Deserialize<T>(string json)
        {
            if (json == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }


        #endregion

        public string Get(string key)
        {
            key = AddKeyPrefix(key);
            return _database.StringGet(key);
        }

        public T Get<T>(string key)
        {
            key = AddKeyPrefix(key);
            var s = _database.StringGet(key);
            return Deserialize<T>(s);
        }

        public bool IsExist(string key)
        {
            key = AddKeyPrefix(key);
            return _database.KeyExists(key);
        }

        public bool Set(string key, string data, int cacheTime = 0)
        {
            key = AddKeyPrefix(key);
            TimeSpan? expiry = null;
            if (cacheTime > 0)
                expiry = TimeSpan.FromMinutes(cacheTime);
            return _database.StringSet(key, data, expiry);
        }

        public bool Set<T>(string key, T data, int cacheTime = 0)
        {
            if (data == null)
                return false;
            key = AddKeyPrefix(key);
            TimeSpan? expiry = null;
            if (cacheTime > 0)
                expiry = TimeSpan.FromMinutes(cacheTime);
            var sdata = Serialize(data);
            return _database.StringSet(key, sdata, expiry);
        }


        public bool SortedSetIncrement(string key, string member, double score, int cacheTime = 0)
        {
            key = AddKeyPrefix(key);
            _database.SortedSetIncrement(key, member, score);
            if (cacheTime > 0)
            {
                TimeSpan expiry = TimeSpan.FromMinutes(cacheTime);
                return _database.KeyExpire(key, expiry);
            }
            return true;
        }

        public double? SortedSetScore(string key, string member)
        {
            key = AddKeyPrefix(key);
            return _database.SortedSetScore(key, member);
        }


        public List<KeyValuePair<string, double>> SortedSetRangeByRankWithScores(string key, int pageindex, int pagesize)
        {
            var result = new List<KeyValuePair<string, double>>();
            key = AddKeyPrefix(key);
            var range = _database.SortedSetRangeByRankWithScores(key, (pageindex - 1) * pagesize, pageindex * pagesize - 1, Order.Descending);
            foreach (var item in range)
            {
                result.Add(new KeyValuePair<string, double>(item.Element, item.Score));
            }
            return result;
        }

        public long SortedSetCombineAndStore(string destinationKey, List<string> keys, int cacheTime = 0)
        {
            destinationKey = AddKeyPrefix(destinationKey);
            var combineKeys = new List<RedisKey>();
            keys.ForEach(key =>
            {
                combineKeys.Add(AddKeyPrefix(key));
            });
            var num = _database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, combineKeys.ToArray());
            if (cacheTime > 0)
            {
                TimeSpan expiry = TimeSpan.FromMinutes(cacheTime);
                _database.KeyExpire(destinationKey, expiry);
            }
            return num;
        }
    }
}
