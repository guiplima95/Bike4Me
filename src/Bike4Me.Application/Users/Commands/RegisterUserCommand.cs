using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public record RegisterUserCommand(
    string Email,
    string Name,
    string Password,
    bool IsAdmin = false) : IRequest<Result<Guid>>;