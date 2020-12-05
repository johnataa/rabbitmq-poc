using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ.Consumers
{
    public abstract class Consumer : IConsumer
    {
        private readonly IModel _model;
        protected readonly ILogger<Consumer> _logger;

        public abstract string QueueName { get; }

        public Consumer(IRabbitMQContext context, IOptions<RabbitMQSettings> options, ILogger<Consumer> logger)
        {
            _logger = logger;
            _model = context.CreateModel();

            var exchange = options.Value.Exchanges.FirstOrDefault(e => e.Queues.Any(q => q.Name == QueueName));

            if (exchange is null)
                throw new Exception("Invalid queue name!");

            var queue = exchange.Queues.FirstOrDefault(q => q.Name == QueueName);

            _model.ExchangeDeclare(exchange.Name, exchange.Type, exchange.IsDurable, exchange.IsAutoDeletable);
            _model.QueueDeclare(QueueName, queue.IsDurable, queue.IsExclusive, queue.IsAutoDeletable);
            _model.QueueBind(QueueName, exchange.Name, queue.RoutingKey);
        }

        public void RegisterEvents()
        {
            _logger.LogInformation("Registering events");
            var consumer = new AsyncEventingBasicConsumer(_model);

            consumer.Received += ConsumerReceivedAsync;
            consumer.Shutdown += ConsumerShutdownAsync;
            consumer.Registered += ConsumerRegisteredAsync;
            consumer.Unregistered += ConsumerUnregisteredAsync;
            consumer.ConsumerCancelled += ConsumerCancelledAsync;

            _model.BasicConsume(QueueName, true, consumer);
        }

        public virtual async Task ConsumerCancelledAsync(object sender, ConsumerEventArgs @event)
        {
            _logger.LogInformation($"Consumer cancelled: {@event.ConsumerTags?.GetValue(0)}");
            await Task.CompletedTask;
        }

        public virtual async Task ConsumerUnregisteredAsync(object sender, ConsumerEventArgs @event)
        {
            _logger.LogInformation($"Consumer unregistered: {@event.ConsumerTags?.GetValue(0)}");
            await Task.CompletedTask;
        }

        public virtual async Task ConsumerRegisteredAsync(object sender, ConsumerEventArgs @event)
        {
            _logger.LogInformation($"Consumer registered: {@event.ConsumerTags?.GetValue(0)}");
            await Task.CompletedTask;
        }

        public virtual async Task ConsumerShutdownAsync(object sender, ShutdownEventArgs @event)
        {
            _logger.LogInformation($"Consumer shutdown: {@event.ReplyText}");
            await Task.CompletedTask;
        }

        public virtual async Task ConsumerReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var content = Encoding.UTF8.GetString(@event.Body.Span);
                _logger.LogInformation($"Message received: {content}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message received but failed to consume: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Consumer disposing");
            _model.Dispose();
        }
    }
}