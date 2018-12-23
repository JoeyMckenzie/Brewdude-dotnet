using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Brewdude.Persistence.Configurations
{
    public class BeerConfiguration : IEntityTypeConfiguration<Beer>
    {
        public void Configure(EntityTypeBuilder<Beer> builder)
        {
            builder.Property(b => b.BeerId).HasColumnName("BeerId");
            
            builder.Property(b => b.Name).IsRequired().HasMaxLength(32);
            
            builder.Property(b => b.Description).HasMaxLength(128);
            
            builder.Property(b => b.Abv).IsRequired();
            
            builder.Property(b => b.Ibu).IsRequired();
            
            builder.Property(b => b.BeerStyle).IsRequired();

            builder
                .HasOne(b => b.Brewery)
                .WithMany(b => b.Beers)
                .HasForeignKey(b => b.BreweryId);

            builder
                .Property(b => b.BeerStyle)
                .HasConversion(new EnumToStringConverter<BeerStyle>());
        }
    }
}