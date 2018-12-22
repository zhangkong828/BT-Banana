using Banana.Models;
using System.Collections.Generic;

namespace Banana.Services
{
    public interface IVideoService
    {
        Video GetVideo(long id);

        List<Video> GetVideoList(IEnumerable<long> ids);

        VideoSource GetVideoSource(long id);

        VideoSource GetVideoSourceByVideo(long videoId);

        List<Video> SearchVideo(string key, int pageIndex, int pageSize, out long totalCount);

        List<Video> GetVideoByClassify(string classify, int pageIndex, int pageSize = 10);

        List<Video> GetVideoByClassify(List<string> classify, int pageIndex, int pageSize = 10);

        List<Video> GetUpdateVideoList(int pageIndex, int pageSize, out long totalCount);
    }
}
