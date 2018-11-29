using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Banana.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _configInfos;
        private readonly IDatabase _database;

        private readonly string DefaultKey;
        public RedisService(IConfiguration config, IDatabase database)
        {
            _configInfos = config;
            _database = database;

            DefaultKey = _configInfos["Redis:defaultKey"];
        }

        #region Private

        private string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }

        private byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var data = memoryStream.ToArray();
                return data;
            }
        }

        private T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
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
            return Deserialize<T>(_database.StringGet(key));
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
            var range = _database.SortedSetRangeByRankWithScores(key, (pageindex - 1) * pagesize, pageindex * pagesize, Order.Descending);
            foreach (var item in range)
            {
                result.Add(new KeyValuePair<string, double>(item.Element, item.Score));
            }
            return result;
        }

        public double? SortedSetScore1(string key, string member)
        {
            key = AddKeyPrefix(key);
            return _database.SortedSetCombineAndStore(key, member);
        }
    }
}
