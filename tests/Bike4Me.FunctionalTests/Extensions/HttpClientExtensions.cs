using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Security.Claims;

namespace Bike4Me.FunctionalTests.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient CreateAuthenticatedClient(
        this TestServer testServer,
        string? email = null,
        string? name = null,
        Guid? clientId = null,
        string[]? roles = null)
    {
        clientId ??= Guid.NewGuid();
        email ??= "testeuser@test.com";
        name ??= "Test User";

        HttpClient client = testServer.CreateIdempotentClient();
        Dictionary<string, dynamic> data = CreateUserTokenAttribute(email, name, clientId, roles);

        return client.SetFakeBearerToken(data);
    }

    private static Dictionary<string, dynamic> CreateUserTokenAttribute(
         string email,
         string name,
         Guid? organizationId = null,
         string[]? roles = null)
    {
        var token = new Dictionary<string, dynamic>
        {
            { "sub", Guid.NewGuid().ToString() },
            { "email", email },
            { ClaimTypes.Email, email },
            { "name", name },
            { ClaimTypes.NameIdentifier, name },
            { ClaimTypes.Name, name },
        };

        if (organizationId.HasValue)
        {
            token.Add("client_organization_id", organizationId.Value);
        }

        if (roles != null)
        {
            token.Add("role", roles);
        }

        return token;
    }

    private static HttpClient CreateIdempotentClient(this TestServer server)
    {
        HttpClient? client = server.CreateClient();
        client.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());

        return client;
    }
}