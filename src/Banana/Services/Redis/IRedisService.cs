using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Services
{
    public interface IRedisService
    {
        string Get(string key);

        T Get<T>(string key);


        bool Set(string key, string data, int cacheTime = 0);

        bool Set<T>(string key, T data, int cacheTime = 0);


        bool IsExist(string key);
    }
}
