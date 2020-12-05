using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;

namespace RabbitMQPoc.BackgroundServices
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private ILogger<RabbitMQBackgroundService> _logger;
        private IMainConsumer _mainConsumer;
        private ILogConsumer _logConsumer;

        public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger, IMainConsumer mainConsumer, ILogConsumer logConsumer)
        {
            _logger = logger;
            _mainConsumer = mainConsumer;
            _logConsumer = logConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
            {
                _logger.LogDebug("Service is stopping.");
            });

            if (!stoppingToken.IsCancellationRequested)
            {
                _mainConsumer.RegisterEvents();
                _logConsumer.RegisterEvents();
            }

            await Task.CompletedTask;
        }
    }
}