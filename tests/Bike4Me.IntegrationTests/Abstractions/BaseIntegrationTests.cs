using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Rentals.Queries.Interfaces;
using Bike4Me.Infrastructure.Database;
using Bogus;
using Microsoft.Extensions.DependencyInjection;

namespace Bike4Me.IntegrationTests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory<Program>>
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory<Program> factory)
    {
        _scope = factory.Services.CreateScope();
        MediatorHandler = _scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        Bike4MeContext = _scope.ServiceProvider.GetRequiredService<Bike4MeContext>();
        RentalsQuery = _scope.ServiceProvider.GetRequiredService<IRentalsQuery>();
        Faker = new Faker();
    }

    protected IMediatorHandler MediatorHandler { get; }
    protected Bike4MeContext Bike4MeContext { get; }
    protected IRentalsQuery RentalsQuery { get; }
    protected Faker Faker { get; }

    public void Dispose() => _scope.Dispose();
}