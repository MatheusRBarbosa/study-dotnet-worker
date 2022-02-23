using System.Text;
using System.Text.Json;

using QueueSimulator.Domain.Interfaces.Infra.Services;
using QueueSimulator.Domain.Models;
using RabbitMQ.Client;

namespace QueueSimulator.Infra.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly ConnectionFactory factory;

        public RabbitService()
        {
            this.factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
        }

        public int Send(Message message)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "queue-default",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var stringMessage = JsonSerializer.Serialize(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringMessage);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "queue-default",
                                         basicProperties: null,
                                         body: bytesMessage);
                }
            }

            return 1;
        }
    }
}