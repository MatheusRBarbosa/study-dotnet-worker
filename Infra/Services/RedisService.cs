using StackExchange.Redis;
using QueueSimulator.Domain.Interfaces.Infra.Services;
using System.Collections;

namespace QueueSimulator.Infra.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer redis;
    private IDatabase db;

    private string primaryKey = "counter";
    private string host = "localhost";
    private int port = 6379;

    public RedisService()
    {
        redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" }
            });

        db = redis.GetDatabase();
    }

    public async Task<int> GetTotal()
    {
        return (int)await db.StringGetAsync(primaryKey);
    }

    public async Task<int> GetPosition(Guid id)
    {
        return (int)await db.StringGetAsync(id.ToString());
    }

    public async Task SetPosition(Guid id, int position)
    {
        await db.StringSetAsync(id.ToString(), position);
    }

    public async Task DecreaseAll()
    {
        var keys = Keys();
        foreach (var key in keys)
        {
            await DecreasePosition(key.ToString()!);
        }
    }

    public async Task DecreaseCounter()
    {
        await db.StringDecrementAsync(primaryKey);
    }

    public async Task IncreaseCounter()
    {
        await db.StringIncrementAsync(primaryKey);
    }

    public async Task DeleteKey(Guid id)
    {
        await db.KeyDeleteAsync(id.ToString());
    }

    private IEnumerable Keys()
    {
        return redis.GetServer(host, port).Keys();
    }

    private async Task DecreasePosition(string id)
    {
        await db.StringDecrementAsync(id);
    }
}