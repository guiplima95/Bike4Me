using Bike4Me.Application.Rentals.Dtos;
using Bike4Me.Application.Rentals.Queries.Interfaces;
using Bike4Me.Domain.Rentals;
using Dapper;
using Npgsql;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Queries;

public sealed class RentalsQuery(string connectionString) : IRentalsQuery
{
    public async Task<Result<RentalResponse>> FindAByIdAsync(Guid id)
    {
        var query = """
            SELECT
                r.id AS Id,
                r.bike_id AS BikeId,
                r.courier_id AS CourierId,
                r.rental_start_date AS RentalStartDate,
                r.rental_end_date AS RentalEndDate,
                r.expected_return_date AS ExpectedReturnDate,
                r.actual_return_date AS ActualReturnDate,
                r.status AS Status,
                r.total_price AS TotalPrice
            FROM bike4me.rentals r
            WHERE r.id = @Id
            """;

        await using var connection = new NpgsqlConnection(connectionString);

        RentalResponse? rental = await connection.QuerySingleOrDefaultAsync<RentalResponse>(
            query,
            new { Id = id }
        );

        if (rental is null)
        {
            return Result.Failure<RentalResponse>(RentalErrors.NotFound);
        }

        return Result.Success(rental);
    }
}