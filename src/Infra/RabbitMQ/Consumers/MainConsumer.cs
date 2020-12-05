using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ.Consumers
{
    public class MainConsumer : Consumer, IMainConsumer
    {
        public override string QueueName => "MainQueue";

        public MainConsumer(
            IRabbitMQContext context,
            IOptions<RabbitMQSettings> options,
            ILogger<MainConsumer> logger
        ) : base(context, options, logger)
        {

        }

        public override async Task ConsumerReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var content = Encoding.UTF8.GetString(@event.Body.Span);
                _logger.LogInformation($"Message received overrided: {content}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message received but failed to consume: {ex.Message}");
            }
        }
    }
}