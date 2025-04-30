using Bike4Me.Domain.Bikes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class MotorcycleModelConfiguration : IEntityTypeConfiguration<BikeModel>
{
    public void Configure(EntityTypeBuilder<BikeModel> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(u => u.Id);

        builder.ComplexProperty(
           u => u.Name,
           b => b.Property(e => e.Value)
            .HasColumnName("name"));

        builder.ComplexProperty(
          u => u.Manufacturer,
          b => b.Property(e => e.Value)
           .IsRequired()
           .HasColumnName("manufacturer"));

        builder.ComplexProperty(
          u => u.Year,
          b => b.Property(e => e.Value)
           .IsRequired()
           .HasColumnName("year"));

        builder.Property(e => e.EngineCapacity);
    }
}