using Bike4Me.Application.Abstractions.Messaging.Interfaces;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikeDeletedEvent(Guid id) : Event
{
    public Guid Id => id;
}