using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Services
{
    public interface IRedisService
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
