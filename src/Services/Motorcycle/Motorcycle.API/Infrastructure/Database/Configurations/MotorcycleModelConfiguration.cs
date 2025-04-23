using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle.API.Domain.MotorcycleAggregate;

namespace Motorcycle.API.Infrastructure.Database.Configurations;

public class MotorcycleModelConfiguration : IEntityTypeConfiguration<MotorcycleModel>
{
    public void Configure(EntityTypeBuilder<MotorcycleModel> builder)
    {
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

        builder.Property(e => e.Engine)
            .HasColumnName("engine");
    }
}