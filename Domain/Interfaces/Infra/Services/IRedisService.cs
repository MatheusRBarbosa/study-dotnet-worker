namespace QueueSimulator.Domain.Interfaces.Infra.Services;
public interface IRedisService
{
    Task<int> GetTotal();
    Task<int> GetPosition(Guid id);
    Task SetPosition(Guid id, int position);
    Task DecreasePosition(Guid id);
    Task DecreaseCounter();
    Task IncreaseCounter();
}