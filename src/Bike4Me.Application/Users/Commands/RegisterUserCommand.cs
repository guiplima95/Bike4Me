using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public record RegisterUserCommand(
    string Email,
    string Name,
    string Password) : IRequest<Result<Guid>>;