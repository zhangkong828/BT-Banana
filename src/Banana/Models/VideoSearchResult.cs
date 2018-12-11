using System.Collections.Generic;

namespace Banana.Models
{
    public class VideoSearchResult
    {
        public string Key { get; set; }
        public List<Video> Result { get; set; }
        public long TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
