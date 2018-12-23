using Banana.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Banana.Services
{
    public class VideoService : IVideoService
    {
        private readonly MongoClient _client;

        private readonly string DBNAME = "TVideo";

        public VideoService(MongoClient client)
        {
            _client = client;
        }

        public IMongoDatabase GetDb(string dbName)
        {
            return _client.GetDatabase(dbName);
        }

        public IMongoCollection<T> GetCollection<T>(string dbName, string collectionName)
        {
            return GetDb(dbName).GetCollection<T>(collectionName);
        }

        public Video GetVideo(long id)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.Where(x => x.Id == id);
            return collection.Find(filter).FirstOrDefault();
        }

        public List<Video> GetVideoList(IEnumerable<long> ids)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.In(x => x.Id, ids);
            return collection.Find(filter).ToList();
        }

        public VideoSource GetVideoSource(long id)
        {
            var collection = GetCollection<VideoSource>(DBNAME, "VideoSource");
            var filter = Builders<VideoSource>.Filter.Where(x => x.Id == id);
            return collection.Find(filter).FirstOrDefault();
        }

        public VideoSource GetVideoSourceByVideo(long videoId)
        {
            var collection = GetCollection<VideoSource>(DBNAME, "VideoSource");
            var filter = Builders<VideoSource>.Filter.Where(x => x.VideoId == videoId);
            return collection.Find(filter).FirstOrDefault();
        }

        public List<Video> SearchVideo(string key, int pageIndex, int pageSize, out long totalCount)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(key));
            totalCount = collection.CountDocuments(filter);
            return collection.Find(filter).SortByDescending(x => x.UpdateTime).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }

        public List<Video> GetVideoByClassify(string classify, int pageIndex, int pageSize, out long totalCount)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.Where(x => x.Classify == classify);
            totalCount = collection.CountDocuments(filter);
            return collection.Find(filter).SortByDescending(x => x.UpdateTime).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }

        public List<Video> GetVideoByClassify(List<string> classify, int pageIndex, int pageSize)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.In(x => x.Classify, classify);
            return collection.Find(filter).SortByDescending(x => x.UpdateTime).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }

        public List<Video> GetUpdateVideoList(int pageIndex, int pageSize, out long totalCount)
        {
            var collection = GetCollection<Video>(DBNAME, "Video");
            var filter = Builders<Video>.Filter.Empty;
            totalCount = collection.CountDocuments(filter);
            return collection.Find(filter).SortByDescending(x => x.UpdateTime).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }
    }
}
