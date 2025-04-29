using Bike4Me.Domain.Couriers;
using Bike4Me.Domain.Motorcycles;
using Bike4Me.Domain.Rentals;
using Bike4Me.Domain.Users;
using Microsoft.EntityFrameworkCore;
using MotorcycleEntity = Bike4Me.Domain.Motorcycles.Motorcycle;

namespace Bike4Me.Infrastructure.Database;

public class Bike4MeContext(DbContextOptions<Bike4MeContext> options) : DbContext(options)
{
    public DbSet<MotorcycleEntity> Motorcycles { get; set; }

    public DbSet<MotorcycleModel> MotorcycleModels { get; set; }

    public DbSet<Rental> Rentals { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Courier> Couriers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Bike4MeContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Bike4Me);
    }
}