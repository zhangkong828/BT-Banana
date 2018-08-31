using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Models
{
    public class MagnetLink
    {
        public MagnetLink()
        {
            Files = new List<FileInfo>();
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
        public List<FileInfo> Files { get; set; }
    }


    public class FileInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }
}


