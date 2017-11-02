using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models.ViewModels
{
    public class BaseSearchResultViewModel<T>
    {
        /// <summary>
        /// 搜索总数
        /// </summary>
        public string TotalCount { get; set; }
        /// <summary>
        /// 搜索耗时
        /// </summary>
        public string FindTime { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKey { get; set; }
        /// <summary>
        /// 搜索结果集
        /// </summary>
        public List<T> Result { get; set; }

        public bool HasResult { get { return Result.Count > 0; } }

        public int PageIndex { get; set; }

    }
}
