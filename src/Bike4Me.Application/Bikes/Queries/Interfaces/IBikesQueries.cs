using Bike4Me.Application.Bikes.Dtos;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Queries.Interfaces;

public interface IBikesQueries
{
    Task<Result<List<BikeResponse>>> FindAllByPlateAsync(string? plate);
}