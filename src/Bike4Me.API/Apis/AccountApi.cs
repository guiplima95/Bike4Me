using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Users.Commands;
using Bike4Me.Domain.Users;

namespace Bike4Me.API.Apis;

public class AccountApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", RegisterUser)
           .Produces<Guid>(StatusCodes.Status201Created)
           .Produces<Guid>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status400BadRequest)
           .WithName("RegisterUser")
           .WithDescription("Register a new user")
           .WithTags(Tags.Accounts);

        app.MapPost("/accounts/login", LoginUser)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("LoginUser")
            .WithDescription("Authenticate user and get JWT token")
            .WithTags(Tags.Accounts);

        app.MapPut("/accounts/{id}/role", UpdateUserRole)
            .RequireAuthorization(IdentityRoles.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateUserRole")
            .WithDescription("Update user role")
            .WithTags(Tags.Accounts);
    }

    private static async Task<IResult> RegisterUser(
        RegisterUserCommand command,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(command);

        return result.Match(
            id =>
            {
                var location = $"/accounts/{id}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }

    private static async Task<IResult> LoginUser(
        LoginUserCommand command,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(command);

        return result.Match(
            token => Results.Ok(new { Token = token }),
            error => Results.Unauthorized());
    }

    private static async Task<IResult> UpdateUserRole(
        Guid id,
        UpdateUserRequest request,
        IMediatorHandler mediator)
    {
        var result = await mediator
            .SendCommand(new UpdateUserRoleCommand(id, request.Role));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }
}