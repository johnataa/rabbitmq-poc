using System;

namespace RabbitMQPoc.Infra.RabbitMQ.Contracts
{
    public interface IProducer : IDisposable
    {
        string ExchangeName { get; }
        void SendMessage<T>(T message, string routingKey = null);
    }
}