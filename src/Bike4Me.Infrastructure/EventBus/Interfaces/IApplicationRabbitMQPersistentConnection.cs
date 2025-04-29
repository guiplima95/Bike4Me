using RabbitMQ.Client;

namespace Bike4Me.Infrastructure.EventBus.Interfaces;

public interface IApplicationRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
}