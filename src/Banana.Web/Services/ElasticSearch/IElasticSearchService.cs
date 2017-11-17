using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Services
{
    public interface IElasticSearchService
    {
        void Search(string key);
    }
}
