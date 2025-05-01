using Bike4Me.Application.Bikes.Dtos;
using Bike4Me.Application.Bikes.Queries.Interfaces;
using Dapper;
using Npgsql;
using SharedKernel;

namespace Bike4Me.Application.Bikes.Queries;

public sealed class BikesQueries(string connectionString) : IBikesQueries
{
    public async Task<Result<List<BikeResponse>>> GetAllByPlateAsync(string? licensePlate)
    {
        var query = """
        SELECT
            b.id AS Id,
            bm.name AS ModelName,
            b.license_plate AS LicensePlate,
            bm.year AS Year
        FROM bike4me.bikes b
        INNER JOIN bike4me.bike_models bm ON bm.id = b.model_id
        WHERE (@Plate IS NULL OR b.license_plate ILIKE @PlatePattern)
        """;

        var platePattern = string.IsNullOrWhiteSpace(licensePlate) ? null : $"%{licensePlate}%";

        await using var connection = new NpgsqlConnection(connectionString);

        var bikes = (await connection.QueryAsync<BikeResponse>(
            query,
            new { Plate = licensePlate, PlatePattern = platePattern }
        )).AsList();

        return Result.Success(bikes);
    }
}