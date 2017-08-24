using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel()
        {
            items = new List<ItemViewModel>();
            searchwords = new List<string>();
        }

        /// <summary>
        /// 搜索总数
        /// </summary>
        public string totalcount { get; set; }
        /// <summary>
        /// 搜索耗时
        /// </summary>
        public string findtime { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 搜索结果集
        /// </summary>
        public List<ItemViewModel> items { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public string currentindex { get; set; }
        /// <summary>
        /// 页码列表
        /// </summary>
        public string[] pageindexlist { get; set; }
        public string pagination_html { get; set; }
        /// <summary>
        /// 大家都在搜
        /// </summary>
        public List<string> searchwords { get; set; }

        public bool HasResult { get { return items.Count > 0; } }
    }
}
