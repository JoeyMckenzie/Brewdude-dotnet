using Brewdude.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeDbContext : IdentityDbContext<BrewdudeUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<UserBeer> UserBeers { get; set; }
        public DbSet<UserBrewery> UserBreweries { get; set; }
        
        public BrewdudeDbContext(DbContextOptions<BrewdudeDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrewdudeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}