using MediatR;

namespace Bike4Me.Application.Abstractions.Messaging.Interfaces;

public abstract class Event : Message, INotification;