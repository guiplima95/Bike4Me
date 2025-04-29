using Microsoft.EntityFrameworkCore;

namespace Bike4Me.Infrastructure.Database.Seed;

public interface IDbContextSeed<TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}