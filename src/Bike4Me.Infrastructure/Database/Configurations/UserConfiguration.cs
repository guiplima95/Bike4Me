using Bike4Me.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bike4Me.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Role)
               .IsRequired()
               .HasDefaultValue(UserRole.Client);

        builder.Property(u => u.PasswordHash)
               .IsRequired();

        builder.ComplexProperty(
            u => u.Name,
            b => b.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name"));

        builder.ComplexProperty(
            u => u.Email,
            b => b.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("email"));
    }
}