using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Commands;
public record CreateMotorcycleCommand(
    string Plate,
    string Color,
    string ModelEngineCapacity,
    string ModelManufacturer,
    string ModelName,
    int ModelYear) : IRequest<Result<Guid>>;