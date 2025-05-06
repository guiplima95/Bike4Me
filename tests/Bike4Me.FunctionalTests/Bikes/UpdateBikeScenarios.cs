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
using Bike4Me.Application.Bikes.Commands;
using System.Net.Http.Json;
using Bike4Me.Application.Bikes.Dtos;

namespace Bike4Me.FunctionalTests.Bikes;

public class UpdateBikeScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task UpdatePlate_ShouldReturnBadRequest_WhenPlateIsInvalid()
    {
        // Arrange
        var bikeId = Guid.NewGuid();
        var request = new BikeLicensePlateRequest("INVALID!");
        var updateRequest = new UpdateBikePlateCommand(bikeId, request.LicensePlate);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(bikeId), updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdatePlate_ShouldReturnNotFound_WhenBikeDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new BikeLicensePlateRequest(LicensePlateFaker.GenerateRandom().Value);
        var updateRequest = new UpdateBikePlateCommand(nonExistentId, request.LicensePlate);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(nonExistentId), updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePlate_ShouldReturnConflict_WhenPlateAlreadyExists()
    {
        // Arrange
        var model = BikeModel.Create(
            Guid.NewGuid(),
            new Domain.Bikes.Name("Fake Name"),
            new Manufacturer("Fake Brand"),
            new Year(2020),
            "");

        var bikeOne = Bike.Create(
            Guid.NewGuid(),
            LicensePlateFaker.GenerateRandom(),
            model.Id,
            Faker.Commerce.Color());

        var bikeTwo = Bike.Create(
            Guid.NewGuid(),
            LicensePlateFaker.GenerateRandom(),
            model.Id,
            Faker.Commerce.Color());

        await DbContext.Bikes.AddRangeAsync(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();

        var request = new BikeLicensePlateRequest(bikeTwo.Plate.Value);
        var updateRequest = new UpdateBikePlateCommand(bikeOne.Id, request.LicensePlate);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(bikeOne.Id), updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        // Cleanup
        DbContext.Bikes.RemoveRange(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePlate_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var bikeId = Guid.NewGuid();
        var newPlate = LicensePlateFaker.GenerateRandom();
        var updateRequest = new UpdateBikePlateCommand(bikeId, newPlate.Value);

        using var client = CreateUnauthenticatedClient();

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(bikeId), updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdatePlate_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var bikeId = Guid.NewGuid();
        var request = new BikeLicensePlateRequest(LicensePlateFaker.GenerateRandom().Value);
        var updateRequest = new UpdateBikePlateCommand(bikeId, request.LicensePlate);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(bikeId), updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdatePlate_ShouldReturnNoContent_WhenSuccessful()
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
            LicensePlateFaker.GenerateOldFormat(),
            model.Id,
            Faker.Commerce.Color());

        await DbContext.Bikes.AddAsync(bike);
        await DbContext.SaveChangesAsync();

        var bikeReport = Bike.Build(bike.Id, bike.Plate.Value, model.Name.Value, model.Year.Value);
        await MongoCollection.InsertOneAsync(bikeReport);

        var request = new BikeLicensePlateRequest(LicensePlateFaker.GenerateMercosulFormat().Value);
        var updateRequest = new UpdateBikePlateCommand(bike.Id, request.LicensePlate);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PutAsJsonAsync(Put.BikePlate(bike.Id), updateRequest);

        await WaitForEventBeingProcessed();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        Bike? updatedBike = await DbContext.Bikes
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bike.Id);

        updatedBike.Should().NotBeNull();
        updatedBike?.Plate.Value.Should().Be(request.LicensePlate);

        var bikeInMongo = await MongoCollection
            .Find(Builders<BikeReport>.Filter.Eq(m => m.Id, bike.Id.ToString()))
            .FirstOrDefaultAsync();

        bikeInMongo.Should().NotBeNull();
        bikeInMongo?.LicensePlate.Should().Be(request.LicensePlate);

        // Cleanup
        var bikeToRemove = await DbContext.Bikes.FirstOrDefaultAsync(b => b.Id == bike.Id);
        if (bikeToRemove is not null)
        {
            DbContext.Bikes.Remove(bikeToRemove!);
        }

        await DbContext.SaveChangesAsync();
    }
}