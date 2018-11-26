using Banana.Models;
using System.Collections.Generic;

namespace Banana.Services.MongoDb
{
    public interface IMongoDbService
    {
        Video GetVideo(long id);

        VideoSource GetVideoSource(long id);

        VideoSource GetVideoSourceByVideo(long videoId);

        List<Video> SearchVideo(string key, int pageIndex, int pageSize = 10);

        List<Video> GetVideoByClassify(string classify, int pageIndex, int pageSize = 10);

        List<Video> GetVideoByClassify(List<string> classify, int pageIndex, int pageSize = 10);
    }
}
