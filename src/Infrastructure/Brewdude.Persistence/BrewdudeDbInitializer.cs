namespace Brewdude.Persistence
{
    using System;
    using System.Linq;
    using Domain.Entities;

    public static class BrewdudeDbInitializer
    {
        private const string UserName = "joey.mckenzie";

        public static void Initialize(BrewdudeDbContext context)
        {
            SeedEntities(context);
        }

        private static void SeedEntities(BrewdudeDbContext context)
        {
            SeedUsers(context, out string userId);
            SeedBreweries(context);
            UpdateBreweryAddressesWithBreweryId(context);
            SeedBeers(context);
            SeedUserBeers(context, userId);
            SeedUserBreweries(context, userId);
        }

        private static void SeedUsers(BrewdudeDbContext context, out string userId)
        {
            var brewdudeUser = new BrewdudeUser
            {
                UserName = UserName
            };

            context.Users.Add(brewdudeUser);
            context.SaveChangesAsync();

            userId = context.Users.
                SingleOrDefault(u => string.Equals(u.UserName, UserName, StringComparison.CurrentCultureIgnoreCase))
                ?.Id;
        }

        private static void SeedBeers(BrewdudeDbContext context)
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

        private static void SeedBreweries(BrewdudeDbContext context)
        {
            var breweries = new[]
            {
                new Brewery
                {
                    Description = "One of Northern California's staple breweries", 
                    Name = "Fall River Brewery", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    Website = "http://fallriverbrewing.com/",
                    Address = new Address
                    {
                        City = "Redding",
                        State = "CA",
                        StreetAddress = "123 Cypress Ave",
                        ZipCode = 96002
                    }
                },
                new Brewery
                {
                    Description = "One of America's staple micro-macro breweries", 
                    Name = "Sierra Nevada Brewing Company", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    Website = "https://www.sierranevada.com/",
                    Address = new Address
                    {
                        City = "Chico",
                        State = "CA",
                        StreetAddress = "123 Chico St",
                        ZipCode = 98765
                    }
                },
                new Brewery
                {
                    Description = "A Davis brewery", 
                    Name = "Sudwerk Brewing Company", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow, 
                    Website = "https://sudwerkbrew.com/",
                    Address = new Address
                    {
                        City = "Davis",
                        State = "CA",
                        StreetAddress = "123 Davis St",
                        ZipCode = 12345
                    }
                }
            };

            context.Breweries.AddRange(breweries);
            context.SaveChanges();
        }

        private static void UpdateBreweryAddressesWithBreweryId(BrewdudeDbContext context)
        {
            var fallRiverBrewery = context.Breweries.FirstOrDefault(b => b.Name == "Fall River Brewery");
            var sierraNevadaBrewery = context.Breweries.FirstOrDefault(b => b.Name == "Sierra Nevada Brewing Company");
            var sudwerkBrewingCompany = context.Breweries.FirstOrDefault(b => b.Name == "Sudwerk Brewing Company");
            var breweries = new[] {fallRiverBrewery, sierraNevadaBrewery, sudwerkBrewingCompany};

            fallRiverBrewery.AddressId = context.Addresses.FirstOrDefault(a => a.City == "Redding").AddressId;
            sierraNevadaBrewery.AddressId = context.Addresses.FirstOrDefault(a => a.City == "Chico").AddressId;
            sudwerkBrewingCompany.AddressId = context.Addresses.FirstOrDefault(a => a.City == "Davis").AddressId;

            context.Breweries.UpdateRange(breweries);
            context.SaveChanges();
        }

        private static void SeedUserBeers(BrewdudeDbContext context, string userId)
        {
            var userBeers = new[]
            {
                new UserBeer { UserId = userId, BeerId = 1 },
                new UserBeer { UserId = userId, BeerId = 3 }
            };

            context.UserBeers.AddRange(userBeers);
            context.SaveChanges();
        }

        private static void SeedUserBreweries(BrewdudeDbContext context, string userId)
        {
            var userBreweries = new[]
            {
                new UserBrewery { UserId = userId, BreweryId = 2 },
                new UserBrewery { UserId = userId, BreweryId = 3 },
                new UserBrewery { UserId = userId, BreweryId = 1 }
            };

            context.UserBreweries.AddRange(userBreweries);
            context.SaveChanges();
        }
    }
}