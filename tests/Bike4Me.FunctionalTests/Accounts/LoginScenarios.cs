using Bike4Me.Domain.Users;
using Bike4Me.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Bike4Me.FunctionalTests.Accounts;

public class LoginScenarios(FunctionalTestWebAppFactory<Program> factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var email = Faker.Internet.Email();
        var password = "Password123!";

        var user = User.Create(
            Guid.NewGuid(),
            Email.Create(email).Value,
            new Name(Faker.Name.FullName()),
            UserRole.Client);

        var hashedPassword = PasswordHasher.Hash(user, password);
        user.SetPasswordHash(hashedPassword);

        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        var loginRequest = new
        {
            Email = email,
            Password = password
        };

        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Login, loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.TryGetProperty("token", out var tokenProperty).Should().BeTrue();
        tokenProperty.GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginRequest = new
        {
            Email = "nonexistent@example.com",
            Password = "wrongpassword"
        };

        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Post.Login, loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}