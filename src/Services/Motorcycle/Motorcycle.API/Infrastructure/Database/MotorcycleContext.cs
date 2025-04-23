using Microsoft.EntityFrameworkCore;
using Motorcycle.API.Domain.MotorcycleAggregate;
using MotorcycleEntity = Motorcycle.API.Domain.MotorcycleAggregate.Motorcycle;

namespace Motorcycle.API.Infrastructure.Database;

public class MotorcycleContext(DbContextOptions<MotorcycleContext> options) : DbContext(options)
{
    public DbSet<MotorcycleEntity> Motorcycles { get; set; }

    public DbSet<MotorcycleModel> MotorcycleModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotorcycleContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Motorcycles);
    }
}