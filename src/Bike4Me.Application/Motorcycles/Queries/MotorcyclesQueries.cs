using Bike4Me.Application.Motorcycles.Dtos;
using Bike4Me.Application.Motorcycles.Queries.Interfaces;
using Dapper;
using Npgsql;
using SharedKernel;

namespace Bike4Me.Application.Motorcycles.Queries;

public class MotorcyclesQueries(string connectionString) : IMotorcyclesQueries
{
    public async Task<Result<List<MotorcycleDto>>> FindAllByPlateAsync(string? plate)
    {
        var query = """
        SELECT
            m.id AS Id,
            mm.name AS ModelName,
            m.plate AS Plate,
            mm.year AS Year
        FROM bike4me.motorcycles m
        INNER JOIN bike4me.motorcycle_models mm ON mm.id = m.model_id
        WHERE (@Plate IS NULL OR m.plate ILIKE @PlatePattern)
        """;

        var platePattern = string.IsNullOrWhiteSpace(plate) ? null : $"%{plate}%";

        await using var connection = new NpgsqlConnection(connectionString);

        var motorcycles = (await connection.QueryAsync<MotorcycleDto>(
            query,
            new { Plate = plate, PlatePattern = platePattern }
        )).AsList();

        return Result.Success(motorcycles);
    }
}