using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Couriers.Commands;
using Bike4Me.Application.Couriers.Dtos;
using MediatR;

namespace Bike4Me.API.Apis.Public;

public class CourierApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/couriers", CreateCourier)
            .WithName("CreateCourier")
            .WithDescription("Create courier")
            .WithTags(Tags.Couries)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/couriers/{id}/cnh", ImportCourierCnh)
            .WithName("ImportCourierCnh")
            .WithDescription("Import courier CNH ")
            .WithTags(Tags.Couries)
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> CreateCourier(
        CreateCourierCommand command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

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
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ImportCourierCnhCommand(id, request.ImagemCnh), cancellationToken);

        return result.Match(
            id =>
            {
                var location = $"/couriers/{result.Value}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }
}