using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application
{
    public interface IBrewdudeDbContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Domain.Entities.Beer> Beers { get; set; }
        DbSet<Domain.Entities.Brewery> Breweries { get; set; }
        DbSet<Domain.Entities.UserBeers> UserBeers { get; set; }
        DbSet<Domain.Entities.UserBreweries> UserBreweries { get; set; }
    }
}