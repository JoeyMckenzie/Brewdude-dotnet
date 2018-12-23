using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brewdude.Persistence.Configurations
{
    public class BreweryConfiguration : IEntityTypeConfiguration<Brewery>
    {
        public void Configure(EntityTypeBuilder<Brewery> builder)
        {
            builder.Property(b => b.BreweryId).HasColumnName("BreweryId");
            
            builder.Property(b => b.Name).HasMaxLength(32).IsRequired();

            builder.Property(b => b.Description).HasMaxLength(128).IsRequired();

            builder.Property(b => b.StreetAddress).HasMaxLength(32).IsRequired();

            builder.Property(b => b.City).IsRequired();

            builder.Property(b => b.State).HasMaxLength(2).IsFixedLength().IsRequired();

            builder.Property(b => b.ZipCode).HasMaxLength(5).IsRequired();
            
            builder
                .HasMany(b => b.Beers)
                .WithOne(b => b.Brewery)
                .IsRequired();
        }
    }
}