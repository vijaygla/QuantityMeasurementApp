using StackExchange.Redis;
using System.Text.Json;

namespace QuantityMeasurementApp.Utilities
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;

        // Inject Redis connection
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        // 🔍 Get data from Redis
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        // 💾 Save data to Redis
        public async Task SetAsync<T>(string key, T data, int expiryMinutes = 10)
        {
            var jsonData = JsonSerializer.Serialize(data);

            await _cache.StringSetAsync(
                key,
                jsonData,
                TimeSpan.FromMinutes(expiryMinutes) // auto expiry
            );
        }

        // ❌ Remove cache (optional use)
        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        // 🧹 Clear all cache (use carefully)
        public async Task ClearAllAsync()
        {
            var endpoints = _cache.Multiplexer.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _cache.Multiplexer.GetServer(endpoint);
                await foreach (var key in server.KeysAsync())
                {
                    await _cache.KeyDeleteAsync(key);
                }
            }
        }
    }
}
