using Bike4Me.Application.Abstractions.Security;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Extensions;
using Bike4Me.Infrastructure.Database;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Bike4Me.FunctionalTests.Abstractions;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory<Program>>
{
    private readonly IServiceScope _scope;
    private readonly FunctionalTestWebAppFactory<Program> _factory;

    public BaseFunctionalTest(FunctionalTestWebAppFactory<Program> factory)
    {
        _scope = factory.Services.CreateScope();
        _factory = factory;

        DbContext = _scope.ServiceProvider.GetRequiredService<Bike4MeContext>();
        MongoCollection = _scope.ServiceProvider.GetRequiredService<IMongoCollection<BikeReport>>();
        PasswordHasher = _scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        Faker = new Faker();
    }

    public static JsonSerializerOptions JsonSerializerOptions
    {
        get
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return options;
        }
    }

    public HttpClient CreateAuthenticatedClient(
        string? email = null,
        string? name = null,
        Guid? organizationId = null,
        string[]? roles = null)
    {
        return _factory.Server.CreateAuthenticatedClient(email, name, organizationId, roles);
    }

    public HttpClient CreateUnauthenticatedClient()
    {
        return _factory.Server.CreateClient();
    }

    public HttpClient CreateClient() => _factory.Server.CreateClient();

    protected Bike4MeContext DbContext { get; }

    protected Faker Faker { get; }

    protected IPasswordHasher<User> PasswordHasher { get; }

    protected IMongoCollection<BikeReport> MongoCollection { get; }

    public static async Task WaitForEventBeingProcessed(TimeSpan? timeSpan = null)
    {
        if (!timeSpan.HasValue)
        {
            timeSpan = TimeSpan.FromSeconds(5);
        }

        await Task.Delay(timeSpan.Value);
    }
}

public static class Post
{
    public const string Login = "api/v1/accounts/login";
    public const string Register = "api/v1/accounts/register";

    public const string Bikes = "api/v1/bikes";
}

public static class Get
{
    public const string Bikes = "api/v1/bikes";
}

public static class Delete
{
    public static string Bike(Guid id) => $"api/v1/bikes/{id}";
}