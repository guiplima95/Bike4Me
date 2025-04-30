using SharedKernel;

namespace Bike4Me.Domain.Users;

public static class UserErrors
{
    public static readonly Error EmailAlreadyExists = Error.Problem(
        "User.EmailAlreadyExists", "A user with this email already exists.");

    public static readonly Error InvalidCredentials = Error.Problem(
        "User.InvalidCredentials", "Invalid email or password.");

    public static readonly Error UserNotFound = Error.Problem(
        "User.UserNotFound", "User not found.");

    public static readonly Error Unauthorized = Error.Problem(
        "User.Unauthorized", "User is not authorized to perform this action.");

    public static readonly Error InvalidRole = Error.Problem(
        "User.InvalidRole", "The specified user role is invalid.");
}