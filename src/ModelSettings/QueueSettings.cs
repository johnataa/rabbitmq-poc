namespace RabbitMQPoc.ModelSettings
{
    public class QueueSettings
    {
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public bool IsDurable { get; set; }
        public bool IsAutoDeletable { get; set; }
        public bool IsExclusive { get; set; }
    }
}