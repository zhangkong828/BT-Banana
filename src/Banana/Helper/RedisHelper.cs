using StackExchange.Redis;

namespace Banana.Helper
{
    public class RedisHelper
    {
        private static readonly object _obj = new object();
        private static IConnectionMultiplexer _connMultiplexer;

        public static IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            if (_connMultiplexer == null || !_connMultiplexer.IsConnected)
            {
                lock (_obj)
                {
                    if (_connMultiplexer == null || !_connMultiplexer.IsConnected)
                    {
                        var config = new ConfigurationOptions
                        {
                            AbortOnConnectFail = false,
                            AllowAdmin = true,
                            ConnectTimeout = 15000,
                            SyncTimeout = 5000,
                            ResponseTimeout = 15000,
                            Password = ConfigurationManager.GetValue("Redis:Password"),
                            EndPoints = { EndPointCollection.TryParse(ConfigurationManager.GetValue("Redis:Connection")) }
                        };
                        _connMultiplexer = ConnectionMultiplexer.Connect(config);
                    }
                }
            }

            return _connMultiplexer;
        }

        public static IDatabase GetDateBase(int db = -1)
        {
            return GetConnectionRedisMultiplexer().GetDatabase(db);
        }
    }
}
