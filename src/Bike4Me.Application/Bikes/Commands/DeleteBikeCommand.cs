using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Commands;

public record DeleteBikeCommand(Guid Id) : IRequest<Result>;