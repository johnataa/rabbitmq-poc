using System.Collections.Generic;

namespace RabbitMQPoc.ModelSettings
{
    public class ExchangeSettings
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDurable { get; set; }
        public bool IsAutoDeletable { get; set; }
        public DeliveryModeEnum DeliveryMode { get; set; }

        public IList<QueueSettings> Queues { get; set; }
    }
}