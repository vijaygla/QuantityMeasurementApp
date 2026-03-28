using RabbitMQ.Client;
using System.Text;

namespace QuantityMeasurementApp.Service
{
    // 🔥 Sends messages to RabbitMQ queue
    public class RabbitMQProducer
    {
        private readonly string _host;
        private readonly string _queueName;

        public RabbitMQProducer(string host, string queueName)
        {
            _host = host;
            _queueName = queueName;
        }

        public void SendMessage(string message)
        {
            // Create connection
            var factory = new ConnectionFactory() { HostName = _host };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Ensure queue exists
            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(message);

            // Publish message
            channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body
            );
        }
    }
}

