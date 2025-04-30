using MediatR;

namespace Bike4Me.Application.Abstractions.Messaging.Interfaces;

public interface IMediatorHandler
{
    Task PublishEvent(Event @event);

    Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> command);

    Task SendCommand(Command command);

    Task<TResponse> SendCommand<TResponse>(Command<TResponse> command);

    Task PublishCommand(Command command);

    Task PublishCommand<TResponse>(Command<TResponse> command);
}