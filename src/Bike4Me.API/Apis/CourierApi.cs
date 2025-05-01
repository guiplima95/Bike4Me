using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Couriers.Commands;
using Bike4Me.Application.Couriers.Dtos;
using Bike4Me.Domain.Users;

namespace Bike4Me.API.Apis;

public class CourierApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/couriers", CreateCourier)
            .RequireAuthorization(IdentityRoles.Client)
            .WithName("CreateCourier")
            .WithDescription("Create a courier")
            .WithTags(Tags.Couries)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/couriers/{id}/cnh", ImportCourierCnh)
            .RequireAuthorization(IdentityRoles.Client)
            .WithName("ImportCourierCnh")
            .WithDescription("Send a image CNH from courier")
            .WithTags(Tags.Couries)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> CreateCourier(
        CreateCourierCommand command,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(command);

        return result.Match(
            id =>
            {
                var location = $"/couriers/{result.Value}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }

    private static async Task<IResult> ImportCourierCnh(
        Guid id,
        ImportCourierCnhRequest request,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(
            new ImportCourierCnhCommand(id, request.ImagemCnh));

        var location = $"/couriers/{result.Value}";

        return result.Match(
            id =>
            {
                var location = $"/couriers/{result.Value}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }
}