using Bike4Me.Application.Abstractions.Messaging;

namespace Bike4Me.Application.Motorcycles.Events;

public sealed class MotorcyclePlateUpdatedDomainEvent(Guid motorcycleId, string plate) : Event
{
    public Guid MotorcycleId => motorcycleId;
    public string Plate => plate;
}