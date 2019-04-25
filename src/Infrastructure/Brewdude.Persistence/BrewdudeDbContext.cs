namespace Brewdude.Persistence
{
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BrewdudeDbContext : IdentityDbContext<BrewdudeUser>
    {
        public BrewdudeDbContext(DbContextOptions<BrewdudeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Beer> Beers { get; set; }

        public DbSet<Brewery> Breweries { get; set; }

        public DbSet<UserBeer> UserBeers { get; set; }

        public DbSet<UserBrewery> UserBreweries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrewdudeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}