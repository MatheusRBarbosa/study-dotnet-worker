using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using QueueSimulator.Domain.Interfaces.Infra.Services;
using QueueSimulator.Domain.Models;

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

        public void ListenEvents()
        {
            QueueDeclare();
            var consumer = new EventingBasicConsumer(currentChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var stringMessage = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", stringMessage);
            };

            Consume(consumer);
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

        private void Consume(EventingBasicConsumer consumer, string queue = "queue-default")
        {
            currentChannel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}