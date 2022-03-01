namespace QueueSimulator.Domain.Interfaces.Infra.Services;
public interface IRedisService
{
    Task<int> GetTotal();
    Task<int> GetPosition(Guid id);

    Task DeleteKey(Guid id);
    Task SetPosition(Guid id, int position);
    Task DecreaseAll();
    Task DecreaseCounter();
    Task IncreaseCounter();
}