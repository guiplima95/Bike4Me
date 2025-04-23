using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Motorcycle.API.Application.Dtos;
using Motorcycle.API.Application.Queries.Interfaces;
using Motorcycle.API.Extensions;
using Motorcycle.API.Infrastructure;
using SharedKernel;

namespace Motorcycle.API.Apis;

public class MotorcycleApi : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("motorcycles", GetMotorcycles)
            .Produces<List<MotorcycleDto>>(StatusCodes.Status200OK)
            .WithName("Get all motorcycles")
            .WithTags(Tags.Motorcycles);
    }

    public static async Task<IResult> GetMotorcycles(IMotorcyclesQueries queries)
    {
        var result = Result.Success(await queries.GetAllMotorcyclesAsync());

        return result.Match(Results.Ok, CustomResults.Problem);
    }
}