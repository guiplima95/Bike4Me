using MediatR;

namespace Bike4Me.Application.Abstractions.Messaging;

public abstract class Event : Message, INotification;