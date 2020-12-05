using System.Collections.Generic;

namespace RabbitMQPoc.ModelSettings
{
    public class RabbitMQSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public string VirtualHost { get; set; }
        public bool HasDispatcherAsync { get; set; }
        public bool HasTopologyRecovery { get; set; }
        public bool HasAutomaticRecovery { get; set; }
        public short NetworkRecoverySeconds { get; set; }

        public IList<ExchangeSettings> Exchanges { get; set; }
    }
}