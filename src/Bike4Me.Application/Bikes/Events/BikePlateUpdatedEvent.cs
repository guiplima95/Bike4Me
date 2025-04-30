using Bike4Me.Application.Abstractions.Messaging.Interfaces;

namespace Bike4Me.Application.Bikes.Events;

public sealed class BikePlateUpdatedEvent(Guid motorcycleId, string plate) : Event
{
    public Guid MotorcycleId => motorcycleId;
    public string Plate => plate;
}