using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brewdude.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserId).HasColumnName("UserId");

            builder.Property(u => u.FirstName).HasMaxLength(32).IsRequired();

            builder.Property(u => u.LastName).HasMaxLength(32).IsRequired();

            builder.Property(u => u.Username).HasMaxLength(16).IsRequired();

            builder.Property(u => u.Email).HasMaxLength(32).IsRequired();

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(u => u.PasswordSalt).IsRequired();
        }
    }
}