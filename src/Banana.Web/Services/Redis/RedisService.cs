using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        public RedisService(IDatabase database)
        {
            _database = database;
        }


        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public Task SetAsync(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
