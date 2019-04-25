namespace Brewdude.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BreweryConfiguration : IEntityTypeConfiguration<Brewery>
    {
        public void Configure(EntityTypeBuilder<Brewery> builder)
        {
            builder.Property(b => b.BreweryId)
                .HasColumnName("BreweryId");

            builder.Property(b => b.Name)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasOne(b => b.Address)
                .WithOne(a => a.Brewery)
                .HasForeignKey<Address>(a => a.BreweryId);

            builder.HasMany(b => b.Beers)
                .WithOne(b => b.Brewery)
                .IsRequired();
        }
    }
}