using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ.Producers
{
    public abstract class Producer : IProducer
    {
        private readonly IModel _model;
        private readonly ExchangeSettings _settings;
        private readonly ILogger<Producer> _logger;

        public abstract string ExchangeName { get; }

        public Producer(IRabbitMQContext rabbitMQContext, IOptions<RabbitMQSettings> options, ILogger<Producer> logger)
        {
            _logger = logger;
            _settings = options.Value.Exchanges.FirstOrDefault(e => e.Name == ExchangeName);

            if (_settings is null)
                throw new Exception("Invalid exchange name!");

            _model = rabbitMQContext.CreateModel();
            _model.ExchangeDeclare(ExchangeName, _settings.Type, _settings.IsDurable, _settings.IsAutoDeletable);
        }

        public virtual void SendMessage<T>(T message, string routingKey = null)
        {
            _logger.LogInformation("Producer sending message");
            var content = BuildContent(message);

            IBasicProperties props = _model.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = (byte)_settings.DeliveryMode;
            _model.BasicPublish(ExchangeName, routingKey ?? "", props, content);
        }

        public void Dispose()
        {
            _logger.LogInformation("Producer disposing");
            _model.Dispose();
        }

        private byte[] BuildContent<T>(T message)
        {
            if (typeof(T) == typeof(string))
                return Encoding.UTF8.GetBytes(message.ToString());

            throw new NotImplementedException();
        }
    }
}