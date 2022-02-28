using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using QueueSimulator.Domain.Interfaces.Infra.Services;

namespace QueueSimulator.Infra.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly ConnectionFactory factory;
        private IModel currentChannel = null!;
        private EventingBasicConsumer currentConsumer = null!;

        public RabbitService()
        {
            this.factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
        }

        public void Send(string message)
        {
            QueueDeclare();
            Console.WriteLine($"[X] Message content: {message}");
            var bytesMessage = Encoding.UTF8.GetBytes(message);
            Publish(bytesMessage);
        }

        public void ListenEvents()
        {
            QueueDeclare();
            currentConsumer = new EventingBasicConsumer(currentChannel);

            currentConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var stringMessage = Encoding.UTF8.GetString(body);
                Console.WriteLine("[x] Received {0}", stringMessage);
                currentChannel.BasicAck(ea.DeliveryTag, false);
            };

            Consume();
        }

        public string? GetMessage(string queue = "queue-default")
        {
            QueueDeclare();
            var ea = currentChannel.BasicGet(queue, true);

            if (ea == null)
            {
                return null;
            }

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
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

            currentChannel.BasicQos(0, 1, false);
        }

        private void Publish(byte[] message, string routeKey = "queue-default")
        {
            currentChannel.BasicPublish(exchange: "",
                            routingKey: routeKey,
                            basicProperties: null,
                            body: message);
        }

        private void Consume(string queue = "queue-default")
        {
            currentChannel.BasicConsume(queue: queue,
                                 autoAck: false,
                                 consumer: currentConsumer);
        }
    }
}