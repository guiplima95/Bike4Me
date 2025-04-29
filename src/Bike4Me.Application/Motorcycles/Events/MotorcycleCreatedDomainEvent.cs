using Bike4Me.Application.Abstractions.Messaging;
using Bike4Me.Domain.Motorcycles;

namespace Bike4Me.Application.Motorcycles.Events;

public sealed class MotorcycleCreatedDomainEvent(MotorcycleReport motorcycle) : Event
{
    public MotorcycleReport Motorcycle => motorcycle;
}