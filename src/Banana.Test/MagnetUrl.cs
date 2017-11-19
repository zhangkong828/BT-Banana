using System;
using System.Collections.Generic;
using System.Text;

namespace Banana.Test
{
    public class MagnetUrl
    {
        public MagnetUrl()
        {
            //Files = new List<File>();
        }
        public string InfoHash { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string[] Tag { get; set; }
        public DateTime CreateTime { get; set; }
        public FileInfo[] Files { get; set; }
    }


    public class File
    {
        public FileInfo[] fileinfo { get; set; }
    }

    public class FileInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
