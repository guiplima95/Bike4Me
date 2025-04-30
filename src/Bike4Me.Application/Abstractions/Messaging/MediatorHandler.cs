using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using MediatR;

namespace Bike4Me.Application.Abstractions.Messaging;

public class MediatorHandler(
    IMediator mediator,
    IApplicationEventBus applicationEventBus) : IMediatorHandler
{
    public async Task PublishEvent(Event notification)
    {
        await applicationEventBus.PublishMessage(notification);
    }

    public async Task SendCommand(Command command)
    {
        await mediator.Send(command);
    }

    public async Task<TResponse> SendCommand<TResponse>(IRequest<TResponse> request)
    {
        return await mediator.Send(request);
    }

    public async Task<TResponse> SendCommand<TResponse>(Command<TResponse> command)
    {
        return await mediator.Send(command);
    }

    public async Task PublishCommand(Command command)
    {
        await applicationEventBus.PublishMessage(command);
    }

    public async Task PublishCommand<TResponse>(Command<TResponse> command)
    {
        await applicationEventBus.PublishMessage(command);
    }
}