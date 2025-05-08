using System.Security.Claims;

namespace Bike4Me.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    internal static Guid UserId(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}