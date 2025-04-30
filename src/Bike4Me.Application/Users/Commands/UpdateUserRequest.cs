using Bike4Me.Domain.Users;

namespace Bike4Me.Application.Users.Commands;

public record UpdateUserRequest(UserRole Role);