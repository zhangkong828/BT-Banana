using Newtonsoft.Json;
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
            //Files = new List<File>();
        }
        [JsonProperty(PropertyName = "infohash")]
        public string InfoHash { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string[] Tag { get; set; }

        [JsonProperty(PropertyName = "createtime")]
        public DateTime CreateTime { get; set; }

        [JsonProperty(PropertyName = "files")]
        public FileInfo[] Files { get; set; }
    }


    public class File
    {
        public FileInfo[] fileinfo { get; set; }
    }

    public class FileInfo
    {
        public object Name { get; set; }
        public object Size { get; set; }
    }
}



public class Rootobject
{
    public File[] file { get; set; }
}

public class File
{
    public long size { get; set; }
    public string name { get; set; }
}

