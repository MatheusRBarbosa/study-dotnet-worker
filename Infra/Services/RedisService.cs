using StackExchange.Redis;
using QueueSimulator.Domain.Interfaces.Infra.Services;

namespace QueueSimulator.Infra.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer redis;
    private IDatabase db;

    private string primaryKey = "counter";

    public RedisService()
    {
        redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" }
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

    public async Task DecreasePosition(Guid id)
    {
        await db.StringDecrementAsync(id.ToString());
    }

    public async Task DecreaseCounter()
    {
        await db.StringDecrementAsync(primaryKey);
    }

    public async Task IncreaseCounter()
    {
        await db.StringIncrementAsync(primaryKey);
    }
}