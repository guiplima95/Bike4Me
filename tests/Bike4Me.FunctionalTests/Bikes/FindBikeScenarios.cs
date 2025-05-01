using Bike4Me.Application.Bikes.Dtos;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Bike4Me.FunctionalTests.Helpers;
using Bogus;
using SharedKernel;

namespace Bike4Me.FunctionalTests.Bikes;

public class FindBikeScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task FindBike_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Client]);

        var response = await client.GetAsync(Get.Bikes);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task FindBike_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        using var client = CreateUnauthenticatedClient();

        var response = await client.GetAsync(Get.Bikes);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task FindBike_ShouldReturnBikes_WhenCalledByAdminWithoutPlateFilter()
    {
        // Arrange
        var model = BikeModel.Create(
            Guid.NewGuid(),
            new Domain.Bikes.Name("Model Fake"),
            new Manufacturer("Brand Fake"),
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

        await DbContext.BikeModels.AddAsync(model);
        await DbContext.Bikes.AddRangeAsync(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        // Act
        var response = await client.GetFromJsonAsync<Result<List<BikeResponse>>>(Get.Bikes, JsonSerializerOptions);

        // Assert
        response?.IsSuccess.Should().BeTrue();
        response?.Value.Should().HaveCountGreaterThan(0);
        response?.Value.Select(b => b.Id).Should().Contain([bikeOne.Id, bikeTwo.Id]);

        // Cleanup
        DbContext.Bikes.RemoveRange(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task FindBike_ShouldReturnBikes_WhenCalledByAdminWithPlateFilter()
    {
        // Arrange
        var model = BikeModel.Create(
            Guid.NewGuid(),
            new Domain.Bikes.Name("Model Fake"),
            new Manufacturer("Brand Fake"),
            new Year(2020),
            "");

        var targetPlate = LicensePlateFaker.GenerateRandom();
        var bikeOne = Bike.Create(
            Guid.NewGuid(),
            targetPlate,
            model.Id,
            Faker.Commerce.Color());

        var bikeTwo = Bike.Create(
            Guid.NewGuid(),
            LicensePlateFaker.GenerateRandom(),
            model.Id,
            Faker.Commerce.Color());

        await DbContext.BikeModels.AddAsync(model);
        await DbContext.Bikes.AddRangeAsync(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();

        using var client = CreateAuthenticatedClient(roles: [IdentityRoles.Admin]);

        var url = $"{Get.Bikes}?plate={targetPlate.Value}";

        // Act
        var response = await client.GetFromJsonAsync<Result<List<BikeResponse>>>(url, JsonSerializerOptions);

        // Assert
        response?.IsSuccess.Should().BeTrue();
        response?.Value.Should().HaveCountGreaterThan(0);
        response?.Value.Count.Should().BeGreaterThan(0);
        response?.Value.Select(b => b.LicensePlate).Should().Contain(targetPlate.Value);

        // Cleanup
        DbContext.BikeModels.Remove(model);
        DbContext.Bikes.RemoveRange(bikeOne, bikeTwo);
        await DbContext.SaveChangesAsync();
    }
}