using Banana.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Services
{
    public interface IElasticSearchService
    {
        MagnetLinkSearchResultViewModel MagnetLinkSearch(string key, int pageIndex, int pageSize);
    }
}
