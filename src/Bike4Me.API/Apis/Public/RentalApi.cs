using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Rentals.Commands;
using Bike4Me.Application.Rentals.Dtos;
using Bike4Me.Application.Rentals.Queries.Interfaces;

namespace Bike4Me.API.Apis.Public;

public class RentalApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/rentals", CreateRental)
            .WithName("CreateRental")
            .WithDescription("Rent a bike")
            .WithTags("Rentals")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapGet("/rentals/{id}", FindRentalById)
            .WithName("FindRentalById")
            .WithDescription("Search rental by id")
            .WithTags(Tags.Rentals)
            .Produces<RentalResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPut("/rentals/{id}/return", ReturnRental)
            .WithName("ReturnRental")
            .WithDescription("Inform return date and calculate total price")
            .WithTags("Rentals")
            .Produces<ReturnRentalResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> CreateRental(
        CreateRentalCommand command,
        IMediatorHandler mediator)
    {
        var result = await mediator.SendCommand(command);

        return result.Match(
            id =>
            {
                var location = $"/rentals/{id}";
                return Results.Created(location, new { Id = id });
            },
            CustomResults.Problem);
    }

    private static async Task<IResult> FindRentalById(
        Guid id,
        IRentalsQuery query)
    {
        var result = await query.FindAByIdAsync(id);

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    private static async Task<IResult> ReturnRental(
        Guid id,
        ReturnRentalRequest request,
        IMediatorHandler mediator)
    {
        var command = new ReturnRentalCommand(id, request.ActualReturnDate);

        var result = await mediator.SendCommand(command);

        return result.Match(Results.Ok, CustomResults.Problem);
    }
}