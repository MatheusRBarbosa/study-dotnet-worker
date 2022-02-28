namespace QueueSimulator.Domain.Interfaces.Infra.Services
{
    public interface IRabbitService
    {
        void Send(string message);
        string? GetMessage(string queue = "queue-default");
        void ListenEvents();
    }
}