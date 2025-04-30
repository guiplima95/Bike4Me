using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Commands;
using Bike4Me.Application.Bikes.Dtos;
using Bike4Me.Application.Bikes.Queries.Interfaces;
using Bike4Me.Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Bike4Me.API.Apis;

public class BikeApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("bikes", FindBike)
            .RequireAuthorization(IdentityRoles.Admin)
            .Produces<List<BikeResponse>>(StatusCodes.Status200OK)
            .WithName("GetBikes")
            .WithDescription("Search for existing bikes")
            .WithTags(Tags.Bikes);

        app.MapPost("bikes", UpdatePlate)
            .RequireAuthorization(IdentityRoles.Admin)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithName("CreateBike")
            .WithDescription("Create a new bike")
            .WithTags(Tags.Bikes);

        app.MapPut("bikes/{id}/plate", UpdatePlate)
            .RequireAuthorization(IdentityRoles.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithName("UpdatePlate")
            .WithDescription("Changing a bike's license plate")
            .WithTags(Tags.Bikes);

        app.MapDelete("bikes/{id}", RemoveBike)
            .RequireAuthorization(IdentityRoles.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("DeleteBike")
            .WithDescription("Delete a bike")
            .WithTags(Tags.Bikes);
    }

    public static async Task<IResult> FindBike(IBikesQueries queries, string? plate = null)
    {
        var result = Result.Success(await queries.FindAllByPlateAsync(plate));

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    public static async Task<IResult> CreateBike(
        CreateBikeCommand command,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(command);

        return result.Match(
            id =>
            {
                var location = $"/bikes/{id}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }

    public static async Task<IResult> UpdatePlate(
        Guid id,
        string plate,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(new UpdateBikePlateCommand(id, plate));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    public static async Task<IResult> RemoveBike(Guid id, IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(new DeleteBikeCommand(id));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }
}