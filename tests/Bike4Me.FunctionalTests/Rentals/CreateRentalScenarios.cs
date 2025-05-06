using Bike4Me.Application.Rentals.Commands;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Couriers;
using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using Bike4Me.FunctionalTests.Helpers;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Bike4Me.FunctionalTests.Rentals;

public class CreateRentalScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)

{
    [Fact]
    public async Task CreateRental_ShouldReturnForbidden_WhenUserIsNotClient()
    {
        // Arrange
        var request = new CreateRentalCommand(
            BikeId: Guid.NewGuid(),
            CourierId: Guid.NewGuid(),
            RentalPlanDays: 5,
            RentalStartDate: DateTime.UtcNow.Date,
            RentalEndDate: DateTime.UtcNow.AddDays(5).Date,
            ExpectedReturnDate: DateTime.UtcNow.AddDays(5).Date);

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Rentals, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateRental_ShouldReturnUnauthorized_WhenUserIsNotLogged()
    {
        // Arrange
        var request = new CreateRentalCommand(
            BikeId: Guid.NewGuid(),
            CourierId: Guid.NewGuid(),
            RentalPlanDays: 5,
            RentalStartDate: DateTime.UtcNow.Date,
            RentalEndDate: DateTime.UtcNow.Date.AddDays(5),
            ExpectedReturnDate: DateTime.UtcNow.Date.AddDays(5));

        using var client = CreateUnauthenticatedClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Rentals, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateRental_ShouldReturnBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var request = new CreateRentalCommand(
            BikeId: Guid.Empty,
            CourierId: Guid.Empty,
            RentalPlanDays: 0,
            RentalStartDate: DateTime.UtcNow.Date,
            RentalEndDate: DateTime.UtcNow.Date.AddDays(-1),
            ExpectedReturnDate: DateTime.UtcNow.Date.AddDays(-1));

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Rentals, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRental_ShouldReturnCreated_WhenDataIsValid()
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

        var courier = Courier.Create(
            Guid.NewGuid(),
            Email.Create("teest@email.com").Value,
            new Domain.Users.Name("Test"),
            Cnh.Create("11122233344", "A").Value,
            new Cnpj("12311123411123"));

        await DbContext.Bikes.AddAsync(bike);
        await DbContext.Couriers.AddAsync(courier);
        await DbContext.SaveChangesAsync();

        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var rentalDays = 15;
        var request = new CreateRentalCommand(
            BikeId: bike.Id,
            CourierId: courier.Id,
            RentalPlanDays: rentalDays,
            RentalStartDate: startDate,
            RentalEndDate: startDate.AddDays(rentalDays),
            ExpectedReturnDate: startDate.AddDays(rentalDays));

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        // Act
        var response = await client.PostAsJsonAsync(Post.Rentals, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // CleanUp
        DbContext.Bikes.Remove(bike);
        DbContext.Couriers.Remove(courier);
        await DbContext.SaveChangesAsync();
    }
}