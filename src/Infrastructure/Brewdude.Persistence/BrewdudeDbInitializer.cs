using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Brewdude.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeDbInitializer
    {
        public static void Initialize(BrewdudeDbContext context)
        {
            var instance = new BrewdudeDbInitializer();
            instance.SeedEntities(context);
        }
        
        public void SeedEntities(BrewdudeDbContext context)
        {
            SeedBreweries(context);
            SeedBeers(context);
//            SeedBreweryIdOnBeersTable(context);
        }

        private void SeedBeers(BrewdudeDbContext context)
        {
            context.Beers.Add(new Beer
            {
                Name = "Hexagenia", 
                Description = "A kickass IPA with all the hoppy goodness a beer lover wants",
                Abv = 7.6, 
                Ibu = 110, 
                BeerStyle = BeerStyle.Ipa, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = context.Breweries.FirstOrDefault(b => b.Name == "Fall River Brewery").BreweryId
            });

            context.Beers.Add(new Beer
            {
                Name = "Lazy Hazy", 
                Description = "A hazy beer straight out of New England", 
                Abv = 8.0, 
                Ibu = 120,
                BeerStyle = BeerStyle.NewEnglandIpa, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = context.Breweries.FirstOrDefault(b => b.Name == "Fall River Brewery").BreweryId
            });

            context.Beers.Add(new Beer
            {
                Name = "Sierra Nevada Pale Ale", 
                Description = "The king of beers, Sierra Nevada's staple Pale Ale",
                Abv = 7.6,
                Ibu = 85, 
                BeerStyle = BeerStyle.PaleAle, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = context.Breweries.FirstOrDefault(b => b.Name == "Sierra Nevada Brewing Company").BreweryId
            });

            context.Beers.Add(new Beer
            {
                Name = "Hoppy Lager", 
                Description = "Our take on the classic lager with a hoppy twist", 
                Abv = 6.2,
                Ibu = 75, 
                BeerStyle = BeerStyle.Lager, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = context.Breweries.FirstOrDefault(b => b.Name == "Sudwerk Brewing Company").BreweryId
            });
            
            context.SaveChanges();
        }

        private void SeedBreweries(BrewdudeDbContext context)
        {
            var breweries = new[]
            {
                new Brewery { Description = "One of Northern California's staple breweries", Name = "Fall River Brewery", City = "Redding", State = "CA", StreetAddress = "123 Cypress Ave", ZipCode = 96002, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Brewery { Description = "One of America's staple micro-macro breweries", Name = "Sierra Nevada Brewing Company", City = "Chico", State = "CA", StreetAddress = "123 Chico St", ZipCode = 98765, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Brewery { Description = "A Davis brewery", Name = "Sudwerk Brewing Company", City = "Davis", State = "CA", StreetAddress = "123 Davis St", ZipCode = 98675, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            
            context.Breweries.AddRange(breweries);
            context.SaveChanges();
        }

//        private void SeedBreweryIdOnBeersTable(BrewdudeDbContext context)
//        {
//           UpdateBreweryIdFromBeerAndBreweryName("Hexagenia", "Fall River Brewery", context);            
//           UpdateBreweryIdFromBeerAndBreweryName("Lazy Hazy", "Fall River Brewery", context);            
//           UpdateBreweryIdFromBeerAndBreweryName("Sierra Nevada Pale Ale", "Sierra Nevada Brewing Company", context);            
//           UpdateBreweryIdFromBeerAndBreweryName("Hoppy Lager", "Sudwerk Brewing Company", context);            
//        }
//
//        private void UpdateBreweryIdFromBeerAndBreweryName(string beerName, string breweryName, BrewdudeDbContext context)
//        {
//            var beer = context.Beers.FirstOrDefault(b => b.Name == beerName);
//            var brewery = context.Breweries.FirstOrDefault(b => b.Name == breweryName);
//
//            if (brewery != null && beer != null)
//            {
//                var beerId = new SqlParameter("BeerId", beer.BeerId);                
//                var breweryId = new SqlParameter("BreweryId", brewery.BreweryId);
//                context.Database.ExecuteSqlCommand("UPDATE Beers SET BreweryID = @BreweryId WHERE BeerId = @BeerId", beerId, breweryId);
//                context.SaveChanges();
//            }
//        }
    }
}