using Bike4Me.Domain.Users;

namespace Bike4Me.Application.Abstractions.Security;

public interface IPasswordHasher<in TUser> where TUser : User
{
    string Hash(TUser user, string password);

    bool Verify(TUser user, string hashedPassword, string providedPassword);
}