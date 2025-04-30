using MediatR;

namespace Bike4Me.Application.Abstractions.Messaging;

public abstract class Command : Message, IRequest;

public abstract class Command<TResponse> : Message, IRequest<TResponse>;