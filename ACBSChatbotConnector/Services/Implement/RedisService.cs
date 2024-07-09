using StackExchange.Redis;

namespace ACBSChatbotConnector.Services.Implement
{
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _conf;
        private string _redisConnectionString;
        private ConnectionMultiplexer _redis;
        public RedisService(IConfiguration conf)
        {
            _conf = conf;
            _redisConnectionString = _conf["Redis:URL"];
            _redis = ConnectionMultiplexer.Connect(_redisConnectionString);
        }

        public async Task Write(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task<string> Read(string key)
        {
            var db = _redis.GetDatabase();
            string _cachedValue = await db.StringGetAsync(key);

            return _cachedValue;
        }
    }
}
