using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Models;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementApp.Service
{
    // 🔥 Background service that listens to queue
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly string _host;
        private readonly string _queueName;
        private readonly IServiceProvider _serviceProvider;
        private readonly Microsoft.Extensions.Logging.ILogger<RabbitMQConsumer> _logger;

        public RabbitMQConsumer(IServiceProvider serviceProvider, IConfiguration configuration, Microsoft.Extensions.Logging.ILogger<RabbitMQConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _host = configuration["RabbitMQ:Host"] ?? "localhost";
            _queueName = configuration["RabbitMQ:QueueName"] ?? "QuantityQueue";
            _logger.LogInformation("RabbitMQConsumer initialized with host: {Host}, queue: {Queue}", _host, _queueName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("RabbitMQConsumer.ExecuteAsync started.");
                var factory = new ConnectionFactory() { HostName = _host };
                IConnection connection = null;
                IModel channel = null;

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Attempting to connect to RabbitMQ at {Host}...", _host);
                        connection = factory.CreateConnection();
                        channel = connection.CreateModel();
                        _logger.LogInformation("Successfully connected to RabbitMQ.");
                        break; // Connected!
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Failed to connect to RabbitMQ: {Message}. Retrying in 5s...", ex.Message);
                        await Task.Delay(5000, stoppingToken);
                    }
                }

                if (stoppingToken.IsCancellationRequested) return;

                // Ensure queue exists
                channel.QueueDeclare(
                    queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message: {Message}", message);

                    try
                    {
                        // 🔄 Convert JSON → Entity
                        var entity = JsonSerializer.Deserialize<QuantityMeasurementEntity>(message);

                        if (entity != null)
                        {
                            // Create scope to use repository
                            using var scope = _serviceProvider.CreateScope();
                            var repo = scope.ServiceProvider.GetRequiredService<IQuantityMeasurementRepository>();

                            // Save to DB
                            repo.Save(entity);
                            _logger.LogInformation("Successfully saved entity to database.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing RabbitMQ message.");
                    }
                };

                // Start consuming
                channel.BasicConsume(
                    queue: _queueName,
                    autoAck: true,
                    consumer: consumer
                );

                _logger.LogInformation("Started consuming from queue: {Queue}", _queueName);

                // Keep service alive
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }

                _logger.LogInformation("RabbitMQConsumer is stopping...");
                channel.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "RabbitMQConsumer encountered a fatal error.");
                throw;
            }
        }
    }
}
