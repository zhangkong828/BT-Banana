using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models
{
    public class MagnetUrl
    {
        public MagnetUrl()
        {
            Files = new List<FileInfo>();
        }
        public string InfoHash { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string[] Tag { get; set; }
        public DateTime CreateTime { get; set; }
        public List<FileInfo> Files { get; set; }
    }


    public class FileInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
