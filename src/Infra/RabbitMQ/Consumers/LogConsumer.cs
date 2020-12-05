using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ.Consumers
{
    public class LogConsumer : Consumer, ILogConsumer
    {
        public override string QueueName => "LogQueue";

        public LogConsumer(
            IRabbitMQContext context,
            IOptions<RabbitMQSettings> options,
            ILogger<LogConsumer> logger
        ) : base(context, options, logger)
        {

        }
    }
}