using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using Xunit;
using Bike4Me.FunctionalTests.Helpers;
using Bogus;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.FunctionalTests.Bikes;

public class RemoveBikeScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task RemoveBike_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        var bikeId = Guid.NewGuid();

        using var client = CreateUnauthenticatedClient();

        var response = await client.DeleteAsync(Delete.Bike(bikeId));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RemoveBike_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var bikeId = Guid.NewGuid();

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        var response = await client.DeleteAsync(Delete.Bike(bikeId));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task RemoveBike_ShouldReturnNotFound_WhenIdNotExists()
    {
        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        var invalidId = Guid.NewGuid();

        var response = await client.DeleteAsync(Delete.Bike(invalidId));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveBike_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var model = BikeModel.Create(
            Guid.NewGuid(),
            new Domain.Bikes.Name("Fake Name"),
            new Manufacturer("Fake Brand"),
            new Year(2020),
            "");

        var bike = Bike.Create(
            Guid.NewGuid(),
            LicensePlateFaker.GenerateRandom(),
            model.Id,
            Faker.Commerce.Color());

        await DbContext.Bikes.AddAsync(bike);
        await DbContext.SaveChangesAsync();

        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, bike.Id.ToString());

        var bikeReport = Bike.Build(bike.Id, bike.Plate.Value, model.Name.Value, model.Year.Value);
        await MongoCollection.InsertOneAsync(bikeReport);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.DeleteAsync(Delete.Bike(bike.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await WaitForEventBeingProcessed();

        var bikeInDb = await DbContext.Bikes.AsNoTracking()
                                            .FirstOrDefaultAsync(b => b.Id == bike.Id);

        var bikeInMongo = await MongoCollection
            .Find(Builders<BikeReport>.Filter.Eq(m => m.Id, bike.Id.ToString()))
            .FirstOrDefaultAsync();

        bikeInDb.Should().BeNull();
        bikeInMongo.Should().BeNull();
    }
}