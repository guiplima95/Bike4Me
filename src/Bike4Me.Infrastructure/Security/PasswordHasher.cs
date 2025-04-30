using Bike4Me.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Bike4Me.Infrastructure.Security;

public class PasswordHasher : Application.Abstractions.Security.IPasswordHasher<User>
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool Verify(User user, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

        return result == PasswordVerificationResult.Success;
    }
}