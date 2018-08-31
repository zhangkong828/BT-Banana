using Banana.Models;
using Banana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Services
{
    public interface IElasticSearchService
    {
        MagnetLinkSearchResultViewModel MagnetLinkSearch(string key, int pageIndex, int pageSize);

        MagnetLink MagnetLinkInfo(string infohash);
    }
}
