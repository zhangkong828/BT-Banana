using Banana.Models;
using System.Collections.Generic;

namespace Banana.Services.MongoDb
{
    public interface IMongoDbService
    {
        Video GetVideo(long id);

        VideoSource GetVideoSource(long id);

        VideoSource GetVideoSourceByVideo(long videoId);

        //List<Video> MatchingVideo(long id);
    }
}
