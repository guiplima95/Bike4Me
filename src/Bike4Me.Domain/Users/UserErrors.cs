using SharedKernel;

namespace Bike4Me.Domain.Users;

public static class UserErrors
{
    public static readonly Error EmailAlreadyExists = Error.Conflict(
        "User.EmailAlreadyExists", "A user with this email already exists.");

    public static readonly Error InvalidCredentials = Error.Failure(
        "User.InvalidCredentials", "Invalid email or password.");

    public static readonly Error UserNotFound = Error.NotFound(
        "User.UserNotFound", "User not found.");

    public static readonly Error InvalidRole = Error.Failure(
        "User.InvalidRole", "The specified user role is invalid.");
}