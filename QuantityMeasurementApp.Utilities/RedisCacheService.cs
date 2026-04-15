using StackExchange.Redis;
using System.Text.Json;

namespace QuantityMeasurementApp.Utilities
{
    public class RedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            
            // Fix: Only pass expiry if it has a value and is greater than zero
            if (expiry.HasValue && expiry.Value > TimeSpan.Zero)
            {
                await _database.StringSetAsync(key, (RedisValue)json, expiry.Value);
            }
            else
            {
                // No expiration
                await _database.StringSetAsync(key, (RedisValue)json);
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            RedisValue value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>((string)value!);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
