using StackExchange.Redis;

namespace MyWebAPI.Services
{
    public class RedisCacheService: IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

       public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;

        }
        public async Task<string> GetCachedDataAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                var cachedData = await db.StringGetAsync(key);
                return cachedData;
            }catch (Exception ex)
            {
                throw new Exception("Unable to get cached data", ex);
            }
        }

        public async Task SetCachedDataAsync(string key, string value, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, expiry);
        }
    }
}
