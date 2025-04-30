using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Bike4Me.FunctionalTests.Accounts;

public class RegisterScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Register_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        var registerRequest = new
        {
            Email = Faker.Internet.Email(),
            Name = Faker.Name.FullName(),
            Password = "Password123!"
        };

        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Register, registerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.TryGetProperty("id", out var idProperty).Should().BeTrue();
        Guid.TryParse(idProperty.GetString(), out var userId).Should().BeTrue();

        // Verifica se o usuário foi salvo no banco
        var userInDb = await DbContext.Users.FindAsync(userId);
        userInDb.Should().NotBeNull();
        userInDb!.Email.Value.Should().Be(registerRequest.Email);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var registerRequest = new
        {
            Email = "invalid-email",
            Name = Faker.Name.FullName(),
            Password = "Password123!"
        };

        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Register, registerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var email = Faker.Internet.Email();

        var existingUser = User.Create(
            Email.Create(email).Value,
            new Name(Faker.Name.FullName()),
            UserRole.Client);

        var hashedPassword = PasswordHasher.Hash(existingUser, "Password123!");
        existingUser.SetPasswordHash(hashedPassword);

        await DbContext.Users.AddAsync(existingUser);
        await DbContext.SaveChangesAsync();

        var registerRequest = new
        {
            Email = email,
            Name = Faker.Name.FullName(),
            Password = "Password123!"
        };

        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Register, registerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}