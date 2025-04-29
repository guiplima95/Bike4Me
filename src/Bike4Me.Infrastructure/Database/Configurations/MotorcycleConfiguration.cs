using Bike4Me.Domain.Motorcycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class MotorcycleConfiguration : IEntityTypeConfiguration<Domain.Motorcycles.Motorcycle>
{
    public void Configure(EntityTypeBuilder<Domain.Motorcycles.Motorcycle> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(m => m.Id);

        builder.OwnsOne(
            m => m.Plate,
            b =>
            {
                b.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("plate");

                b.HasIndex(p => p.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Motorcycles_Plate_Unique");
            });

        builder.Property(m => m.Status)
            .IsRequired()
            .HasDefaultValue(MotorcycleStatus.Available)
            .HasColumnName("status");

        builder.Property(m => m.Color)
            .HasColumnName("color");
    }
}