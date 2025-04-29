using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Couriers.Commands;
public sealed record CreateCourierCommand(
    string Name,
    string Email,
    string Cnpj,
    DateTime DateBirthday,
    string CnhNumber,
    string CnhType,
    string ImagemCnh) : IRequest<Result<Guid>>;