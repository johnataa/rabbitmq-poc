using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ.Producers
{
    public class MainProducer : Producer, IMainProducer
    {
        public override string ExchangeName => "MainExchange";

        public MainProducer(
            IRabbitMQContext rabbitMQContext,
            IOptions<RabbitMQSettings> options,
            ILogger<MainProducer> logger
        ) : base(rabbitMQContext, options, logger)
        {

        }
    }
}