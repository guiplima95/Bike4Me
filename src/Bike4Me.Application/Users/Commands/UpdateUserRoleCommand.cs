using Bike4Me.Domain.Users;
using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Users.Commands;

public record UpdateUserRoleCommand(
    Guid UserId, UserRole Role) : IRequest<Result>;