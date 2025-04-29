using Bike4Me.Infrastructure.Database.Seed;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.API.Extensions;

public static class DatabaseSeedingExtensions
{
    public static async Task<IApplicationBuilder> SeedDatabaseAsync<TContext>(
        this IApplicationBuilder app,
        IDbContextSeed<TContext> seeder)
        where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        await seeder.SeedAsync(
            context,
            scope.ServiceProvider,
            CancellationToken.None);

        return app;
    }
}