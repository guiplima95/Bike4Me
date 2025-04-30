namespace Bike4Me.Application.Abstractions.Messaging.Interfaces;

public interface IApplicationEventBus : IDisposable
{
    Task PublishMessage<T>(T message) where T : Message;

    Task StartConsumer();

    Task StopConsumer();
}