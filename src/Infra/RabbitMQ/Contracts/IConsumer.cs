using System;

namespace RabbitMQPoc.Infra.RabbitMQ.Contracts
{
    public interface IConsumer : IDisposable
    {
        string QueueName { get; }
        void RegisterEvents();
    }
}