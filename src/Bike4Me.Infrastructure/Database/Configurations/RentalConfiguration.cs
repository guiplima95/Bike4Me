using Bike4Me.Domain.Rentals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.BikeId)
            .IsRequired()
            .HasColumnName("bike_id");

        builder.Property(r => r.CourierId)
            .IsRequired()
            .HasColumnName("courier_id");

        builder.ComplexProperty(
            r => r.RentalPlan,
            b =>
            {
                b.Property(p => p.Days)
                    .IsRequired()
                    .HasColumnName("rental_plan_days");

                b.Property(p => p.DailyRate)
                    .IsRequired()
                    .HasColumnName("rental_plan_daily_rate");

                b.Property(p => p.PenaltyPercentage)
                    .IsRequired()
                    .HasColumnName("rental_plan_penalty_percentage");

                b.Property(p => p.AdditionalDailyFee)
                    .IsRequired()
                    .HasColumnName("rental_plan_additional_daily_fee");
            });

        builder.Property(r => r.RentalStartDate)
            .IsRequired()
            .HasColumnName("rental_start_date");

        builder.Property(r => r.ExpectedReturnDate)
            .IsRequired()
            .HasColumnName("expected_return_date");

        builder.Property(r => r.ActualReturnDate)
            .HasColumnName("actual_return_date");

        builder.Property(r => r.Status)
            .IsRequired()
            .HasColumnName("status");

        builder.Property(r => r.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("total_price");
    }
}