using Azure.Core;
using Bike4Me.Application.Bikes.Commands;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Bike4Me.FunctionalTests.Bikes;

public class CreateBikeScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task CreateBike_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        CreateBikeCommand request = new(
            "ABC1Q99", "blue", "250.2", "Honda", "CTX", 2020);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Bikes, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateBike_ShouldReturnUnauthorized_WhenUserIsNotLogged()
    {
        // Arrange
        CreateBikeCommand request = new(
            "ABC1Q99", "blue", "250.2", "Honda", "CTX", 2020);

        using var client = CreateUnauthenticatedClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Bikes, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateBike_ShouldReturnBadRequest_WhenPlateIsInvalidFormat()
    {
        // Arrange
        var existingBike = Bike.Create(Guid.NewGuid(), new LicensePlate("DUPL122"), Guid.NewGuid(), "Color");
        await DbContext.Bikes.AddAsync(existingBike);
        await DbContext.SaveChangesAsync();

        var request = new CreateBikeCommand(
            Plate: "DUPL122",
            Color: "red",
            ModelEngineCapacity: "300",
            ModelManufacturer: "Yamaha",
            ModelName: "YZF",
            ModelYear: 2021);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Bikes, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Cleanup
        DbContext.Bikes.Remove(existingBike);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateBike_ShouldReturnConflict_WhenPlateAlreadyExists()
    {
        // Arrange
        var existingBike = Bike.Create(Guid.NewGuid(), new LicensePlate("ABC1Q44"), Guid.NewGuid(), "Color");
        await DbContext.Bikes.AddAsync(existingBike);
        await DbContext.SaveChangesAsync();

        var request = new CreateBikeCommand(
            Plate: "ABC1Q44",
            Color: "red",
            ModelEngineCapacity: "300",
            ModelManufacturer: "Yamaha",
            ModelName: "YZF",
            ModelYear: 2021);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Bikes, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        // Cleanup
        DbContext.Bikes.Remove(existingBike);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateBike_ShouldCreateBike_WhenDataIsValid()
    {
        // Arrange
        CreateBikeCommand request = new(
            "ABC1Q88", "blue", "250.2", "Honda", "CTX", 2020);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Bikes, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var bikeInDb = await DbContext.Bikes.FirstOrDefaultAsync(b => b.Plate.Value == request.Plate);
        bikeInDb.Should().NotBeNull();

        var filter = Builders<BikeReport>.Filter.Eq(m => m.Id, bikeInDb?.Id.ToString());
        var bikeInMongo = await MongoCollection.Find(filter).FirstOrDefaultAsync();
        bikeInMongo.Should().NotBeNull();

        // Cleanup
        DbContext.Bikes.Remove(bikeInDb!);
        await DbContext.SaveChangesAsync();

        if (bikeInMongo is not null)
        {
            await MongoCollection.DeleteOneAsync(filter);
        }
    }
}