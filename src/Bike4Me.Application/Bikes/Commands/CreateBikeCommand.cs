using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;
public record CreateBikeCommand(
    string Plate,
    string Color,
    string ModelEngineCapacity,
    string ModelManufacturer,
    string ModelName,
    int ModelYear) : IRequest<Result<Guid>>;