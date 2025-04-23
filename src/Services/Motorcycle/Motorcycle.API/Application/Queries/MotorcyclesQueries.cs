using Dapper;
using Motorcycle.API.Application.Dtos;
using Motorcycle.API.Application.Queries.Interfaces;
using Npgsql;

namespace Motorcycle.API.Application.Queries;

public class MotorcyclesQueries(string connectionString) : IMotorcyclesQueries
{
    public async Task<List<MotorcycleDto>> GetAllMotorcyclesAsync()
    {
        const string query = """

                SELECT
                    mm.name AS Name,
                    m.plate AS Plate,
                    m.color AS Color,
                    m.status AS Status
                FROM motorcycles m
                INNER JOIN motorcycles_models mm ON mm.id = m.id
            """;

        await using var connection = new NpgsqlConnection(connectionString);

        return [.. (await connection.QueryAsync<MotorcycleDto>(query))];
    }
}