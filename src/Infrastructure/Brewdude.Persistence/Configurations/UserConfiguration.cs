using Brewdude.Common.Constants;
using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Brewdude.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserId).HasColumnName("UserId");

            builder.Property(u => u.FirstName).HasMaxLength(BrewdudeConstants.MaxNameLength).IsRequired();

            builder.Property(u => u.LastName).HasMaxLength(BrewdudeConstants.MaxNameLength).IsRequired();

            builder.Property(u => u.Username).HasMaxLength(BrewdudeConstants.MaxUsernameLength).IsRequired();

            builder.Property(u => u.Email).HasMaxLength(BrewdudeConstants.MaxEmailLength).IsRequired();

            builder.Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<Role>())
                .IsRequired();

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(u => u.PasswordSalt).IsRequired();
        }
    }
}