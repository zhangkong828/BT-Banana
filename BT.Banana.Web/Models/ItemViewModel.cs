using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.Banana.Web.Models
{
    public class ItemViewModel
    {
        public ItemViewModel()
        {
            searchkeywords = new List<string>();
            soyoulike = new List<ItemInfo>();
            filelist = new List<FileInfo>();
            filedes = new List<string>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 磁力链接哈希值
        /// </summary>
        public string hash { get; set; }
        /// <summary>
        /// 磁力链接地址
        /// </summary>
        public string magnet { get; set; }
        public string id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public string filecount { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string filesize { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string createtime { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string updatetime { get; set; }
        /// <summary>
        /// 下载次数
        /// </summary>
        public string downloadcount { get; set; }
        /// <summary>
        /// 文件信息（显示文件列表的TOP5）
        /// </summary>
        public List<string> filedes { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public List<string> searchkeywords { get; set; }
        /// <summary>
        /// 你可能也喜欢
        /// </summary>
        public List<ItemInfo> soyoulike { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileInfo> filelist { get; set; }
    }

    public class FileInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string size { get; set; }
    }

    public class ItemInfo
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}