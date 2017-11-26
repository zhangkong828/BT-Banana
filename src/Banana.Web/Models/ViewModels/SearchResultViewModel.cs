using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models.ViewModels
{
    public class SearchResultViewModel : BaseSearchResultViewModel<TencentItem>
    {
    }

    public class TencentItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
