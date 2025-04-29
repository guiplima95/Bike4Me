using Bike4Me.Domain.Couriers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class CourierConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(c => c.Id);

        builder.ComplexProperty(
            c => c.Name,
            b => b.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name"));

        builder.ComplexProperty(
            c => c.Email,
            b => b.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("email"));

        builder.OwnsOne(
            c => c.Cnh,
            b =>
            {
                b.Property(cnh => cnh.Number)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("cnh_number");

                b.Property(cnh => cnh.Category)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("cnh_category");
            });
    }
}