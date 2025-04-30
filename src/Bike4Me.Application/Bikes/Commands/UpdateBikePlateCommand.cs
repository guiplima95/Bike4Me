using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public record UpdateBikePlateCommand(Guid Id, string Plate) : IRequest<Result>;