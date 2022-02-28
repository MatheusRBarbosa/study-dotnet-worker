using QueueSimulator.Domain.Models;

namespace QueueSimulator.Domain.Interfaces.Infra.Services
{
    public interface IRabbitService
    {
        int Send(Message message);
        Message Receive();
    }
}