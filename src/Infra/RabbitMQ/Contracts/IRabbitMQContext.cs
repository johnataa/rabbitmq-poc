using System;
using RabbitMQ.Client;

namespace RabbitMQPoc.Infra.RabbitMQ.Contracts
{
    public interface IRabbitMQContext : IDisposable
    {
        IModel CreateModel();
    }
}