using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<UserBeers> UserBeers { get; set; }
        public DbSet<UserBreweries> UserBreweries { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<BrewdudeUser> BrewdudeUsers { get; set; }

        public BrewdudeDbContext(DbContextOptions<BrewdudeDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrewdudeDbContext).Assembly);
        }
    }
}