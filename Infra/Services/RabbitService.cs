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
        private IModel currentChannel = null!;

        public RabbitService()
        {
            this.factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
        }

        public int Send(Message message)
        {
            QueueDeclare();
            var stringMessage = JsonSerializer.Serialize(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringMessage);
            Publish(bytesMessage);
            return 1;
        }

        public Message Receive()
        {
            Message message = null!;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                }
            }

            return message;
        }

        private void QueueDeclare(string queue = "queue-default")
        {
            var connection = factory.CreateConnection();
            currentChannel = connection.CreateModel();

            currentChannel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        private void Publish(byte[] message, string routeKey = "queue-default")
        {
            currentChannel.BasicPublish(exchange: "",
                            routingKey: routeKey,
                            basicProperties: null,
                            body: message);
        }
    }
}