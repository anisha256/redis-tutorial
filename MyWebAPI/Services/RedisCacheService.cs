using StackExchange.Redis;

namespace MyWebAPI.Services
{
    public class RedisCacheService : IRedisCacheService
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
                var cachedData = await _redis.GetDatabase().StringGetAsync(key);
                return cachedData;


            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get cached data", ex);
            }
        }

        public async Task SetCachedDataAsync(string key, string value, TimeSpan expiry)
        {
            await _redis.GetDatabase().StringSetAsync(key, value, expiry);
        }


        public async Task RemoveAsync(string key)
        {
            await _redis.GetDatabase().KeyDeleteAsync(key);
        }

    }
}
