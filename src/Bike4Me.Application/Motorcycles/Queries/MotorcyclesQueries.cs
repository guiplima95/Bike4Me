using Bike4Me.Application.Motorcycles.Dtos;
using Bike4Me.Application.Motorcycles.Queries.Interfaces;
using Bike4Me.Domain.Motorcycles;
using Dapper;
using Npgsql;
using SharedKernel;
using System.Runtime.CompilerServices;

namespace Bike4Me.Application.Motorcycles.Queries;

public class MotorcyclesQueries(string connectionString) : IMotorcyclesQueries
{
    public async Task<Result<List<MotorcycleDto>>> GetAllAsync()
    {
        const string query = """
            SELECT
                m.id AS Id,
                mm.name AS ModelName,
                m.plate AS Plate,
                mm.year AS Year
            FROM bike4me.motorcycles m
            INNER JOIN bike4me.motorcycle_models mm ON mm.id = m.model_id
            """;

        await using var connection = new NpgsqlConnection(connectionString);

        var motorcycles = (await connection.QueryAsync<MotorcycleDto>(query)).AsList();

        return Result.Success(motorcycles);
    }

    public async Task<Result<MotorcycleDto>> GetByPlateAsync(string plate)
    {
        const string query = """
        SELECT
            m.id AS Id,
            mm.name AS ModelName,
            m.plate AS Plate,
            mm.year AS Year
        FROM bike4me.motorcycles m
        INNER JOIN bike4me.motorcycle_models mm ON mm.id = m.model_id
        WHERE m.plate = @Plate
        """;

        await using var connection = new NpgsqlConnection(connectionString);

        MotorcycleDto? motorcycle = await connection
            .QuerySingleOrDefaultAsync<MotorcycleDto>(query, new { Plate = plate });

        if (motorcycle is null)
        {
            return Result.Failure<MotorcycleDto>(MotorcycleErrors.NotFoundByPlate);
        }

        return motorcycle;
    }
}