using System.Text.Json;
using Catalog.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Catalog.Infrastructure.Caching;

public class RedisCacheService : ICacheService, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisCacheService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _database = _redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default!;

        return JsonSerializer.Deserialize<T>(value!)!;
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _database.KeyDeleteAsync(key);
    }

    public void Dispose()
    {
        _redis.Dispose();
    }
}