using Bike4Me.Domain.Bikes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class BikeConfiguration : IEntityTypeConfiguration<Bike>
{
    public void Configure(EntityTypeBuilder<Bike> builder)
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
                    .HasColumnName("license_plate");

                b.HasIndex(p => p.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Bikes_License_Plate_Unique");
            });

        builder.Property(m => m.Status)
            .IsRequired()
            .HasDefaultValue(BikeStatus.Available)
            .HasColumnName("status");

        builder.Property(m => m.Color)
            .HasColumnName("color");
    }
}