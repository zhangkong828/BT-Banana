using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models.ViewModels
{
    public class BaseSearchResultViewModel<T>
    {
        public BaseSearchResultViewModel()
        {
            SearchResult = new List<T>();
        }
        public string SearchId { get; set; }
        public string SearchKey { get; set; }

        public string FindTime { get; set; }

        public List<T> SearchResult { get; set; }

        public bool HasResult { get { return SearchResult.Count > 0; } }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageTotals { get; set; }

        public int Totals { get; set; }

    }
}
