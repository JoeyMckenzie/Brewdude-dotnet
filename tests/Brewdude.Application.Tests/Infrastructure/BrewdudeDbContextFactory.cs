using System;
using System.Linq;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class BrewdudeDbContextFactory
    {
        public static BrewdudeDbContext Create()
        {
            var options = new DbContextOptionsBuilder<BrewdudeDbContext>()
                .UseInMemoryDatabase("Brewdude.Application.Tests.Db")
                .Options;
            
            var context = new BrewdudeDbContext(options);
            context.Database.EnsureCreated();
            
            SeedBreweries(context);
            SeedBeers(context);
            SeedUsers(context);
            SeedUsersBeers(context);
            
            return context;
        }

        public static void Destroy(BrewdudeDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private static void SeedUsers(BrewdudeDbContext context)
        {
            context.Users.Add(new Domain.Entities.User
            {
                Username = "joey.mckenzie27",
            });
            context.SaveChanges();
        }

        private static void SeedUsersBeers(BrewdudeDbContext context)
        {
            context.UserBeers.AddRange(new[]
            {
                new Domain.Entities.UserBeers { UserId = 1, BeerId = 3 },
                new Domain.Entities.UserBeers { UserId = 1, BeerId = 1 }
            });
            context.SaveChanges();
        }

        private static void SeedBreweries(BrewdudeDbContext context)
        {
            var breweries = new[]
            {
                new Domain.Entities.Brewery { Description = "One of Northern California's staple breweries", Name = "Fall River Brewery", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Domain.Entities.Brewery { Description = "One of America's staple micro-macro breweries", Name = "Sierra Nevada Brewing Company", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Domain.Entities.Brewery { Description = "A Davis brewery", Name = "Sudwerk Brewing Company", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            context.Breweries.AddRange(breweries);
        }
        
        private static void SeedBeers(BrewdudeDbContext context)
        {
            context.Beers.Add(new Domain.Entities.Beer
            {
                Name = "Hexagenia", 
                Description = "A kickass IPA with all the hoppy goodness a beer lover wants",
                Abv = 7.6, 
                Ibu = 110, 
                BeerStyle = BeerStyle.Ipa, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = 1
            });

            context.Beers.Add(new Domain.Entities.Beer
            {
                Name = "Lazy Hazy", 
                Description = "A hazy beer straight out of New England", 
                Abv = 8.0, 
                Ibu = 120,
                BeerStyle = BeerStyle.NewEnglandIpa, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = 1
            });

            context.Beers.Add(new Domain.Entities.Beer
            {
                Name = "Sierra Nevada Pale Ale", 
                Description = "The king of beers, Sierra Nevada's staple Pale Ale",
                Abv = 7.6,
                Ibu = 85, 
                BeerStyle = BeerStyle.PaleAle, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = 2
            });

            context.Beers.Add(new Domain.Entities.Beer
            {
                Name = "Hoppy Lager", 
                Description = "Our take on the classic lager with a hoppy twist", 
                Abv = 6.2,
                Ibu = 75, 
                BeerStyle = BeerStyle.Lager, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = 3
            });
            context.SaveChanges();
        }
    }
}