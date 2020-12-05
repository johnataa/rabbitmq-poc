using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc.Infra.RabbitMQ
{
    public class RabbitMQContext : IRabbitMQContext
    {
        private readonly ILogger<RabbitMQContext> _logger;
        private readonly IConnection _connection;

        public RabbitMQContext(ILogger<RabbitMQContext> logger, IOptions<RabbitMQSettings> options)
        {
            _logger = logger;

            var connectionFactory = new ConnectionFactory
            {
                UserName = options.Value.Username,
                Password = options.Value.Password,
                VirtualHost = options.Value.VirtualHost,
                DispatchConsumersAsync = options.Value.HasDispatcherAsync,
                TopologyRecoveryEnabled = options.Value.HasTopologyRecovery,
                AutomaticRecoveryEnabled = options.Value.HasAutomaticRecovery,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(options.Value.NetworkRecoverySeconds)
            };

            _connection = connectionFactory.CreateConnection();
        }

        public IModel CreateModel()
        {
            try
            {
                return _connection.CreateModel();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao tentar abrir conexão!");
                throw ex;
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing context");
            _connection.Dispose();
        }
    }
}