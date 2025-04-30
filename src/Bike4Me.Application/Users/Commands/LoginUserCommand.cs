using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<string>>;
