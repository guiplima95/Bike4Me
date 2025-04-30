using Bike4Me.Domain.Users;

namespace Bike4Me.Application.Abstractions.Security;

public interface ITokenService
{
    string GenerateToken(User user);
}