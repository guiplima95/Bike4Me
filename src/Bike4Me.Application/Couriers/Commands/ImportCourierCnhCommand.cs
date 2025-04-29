using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Couriers.Commands;
public sealed record ImportCourierCnhCommand(Guid CourierId, string ImagemCnh) : IRequest<Result<Guid>>;