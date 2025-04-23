using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle.API.Domain.MotorcycleAggregate;
using MotorcycleEntity = Motorcycle.API.Domain.MotorcycleAggregate.Motorcycle;

namespace Motorcycle.API.Infrastructure.Database.Configurations;

public class MotorcycleConfiguration : IEntityTypeConfiguration<MotorcycleEntity>
{
    public void Configure(EntityTypeBuilder<MotorcycleEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.ComplexProperty(
           u => u.Plate,
           b => b.Property(e => e.Value)
            .HasColumnName("plate"));

        builder.Property(e => e.Status)
           .IsRequired()
           .HasDefaultValue(MotorcycleStatus.Available)
           .HasColumnName("status");

        builder.Property(e => e.Color)
            .HasColumnName("color");
    }
}