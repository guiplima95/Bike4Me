using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.Application.Motorcycles.Commands;
using Bike4Me.Application.Motorcycles.Dtos;
using Bike4Me.Application.Motorcycles.Queries.Interfaces;
using MediatR;
using SharedKernel;

namespace Bike4Me.API.Apis.Admin;

public class MotorcycleApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("motorcycles", GetMotorcycles)
            .Produces<List<MotorcycleDto>>(StatusCodes.Status200OK)
            .WithName("GetMotorcycles")
            .WithDescription("Get motorcycles")
            .WithTags(Tags.Motorcycles);

        app.MapPost("motorcycles", CreateMotorcycle)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithName("CreateMotorcycle")
            .WithDescription("Create motorcycle")
            .WithTags(Tags.Motorcycles);

        app.MapPut("motorcycles/{id}/plate", UpdateMotorcyclePlate)
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status409Conflict)
           .WithName("UpdateMotorcyclePlate")
           .WithDescription("Update motorcycle")
           .WithTags(Tags.Motorcycles);
    }

    public static async Task<IResult> GetMotorcycles(IMotorcyclesQueries queries, string? plate = null)
    {
        var result = Result.Success(await queries.FindAllByPlateAsync(plate));

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    public static async Task<IResult> CreateMotorcycle(
        CreateMotorcycleCommand command,
        ISender sender)
    {
        var result = await sender.Send(command);

        return result.Match(
            id =>
            {
                var location = $"/motorcycles/{id}";
                return Results.Created(location, new { Id = id });
            }, CustomResults.Problem);
    }

    public static async Task<IResult> UpdateMotorcyclePlate(Guid id, string plate, ISender sender)
    {
        var result = await sender.Send(new UpdateMotorcyclePlateCommand(id, plate));

        return result.Match(Results.NoContent, CustomResults.Problem);
    }
}