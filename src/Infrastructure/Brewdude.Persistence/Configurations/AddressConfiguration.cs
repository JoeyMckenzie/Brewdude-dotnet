using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brewdude.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.AddressId).HasColumnName("AddressId");

            builder.Property(a => a.City).HasMaxLength(32).IsRequired();
            
            builder.Property(a => a.StreetAddress).HasMaxLength(32).IsRequired();

            builder.Property(a => a.State).HasMaxLength(2).IsFixedLength().IsRequired();

            builder.Property(a => a.ZipCode).HasMaxLength(5).IsFixedLength().IsRequired();

            builder.HasOne(a => a.Brewery)
                .WithOne(b => b.Address)
                .HasForeignKey<Brewery>(b => b.AddressId);
        }
    }
}