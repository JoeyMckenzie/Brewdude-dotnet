using System;
using System.Linq;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class BrewdudeDbIdentityContextFactory
    {
        private const string UserName = "joey.mckenzie";

        public static BrewdudeDbIdentityContext Create(out string testUserId)
        {
            var options = new DbContextOptionsBuilder<BrewdudeDbContext>()
                .UseInMemoryDatabase("Brewdude.Application.Tests.Db")
                .Options;
            
            var identityOptions = new DbContextOptionsBuilder<BrewdudeDbIdentityContext>()
                .UseInMemoryDatabase("Brewdude.Application.Tests.Db")
                .Options;
            
            var context = new BrewdudeDbContext(options);
            context.Database.EnsureCreated();
            
            var identityContext = new BrewdudeDbIdentityContext(identityOptions);
            identityContext.Database.EnsureCreated();
            
            SeedUsers(identityContext, out string userId);
            SeedUserBeers(context, userId);
            SeedUserBreweries(context, userId);
            testUserId = userId;
            
            return identityContext;
        }
        
        public static void Destroy(BrewdudeDbIdentityContext identityContext)
        {
            identityContext.Database.EnsureDeleted();
            identityContext.Dispose();
        }
        
        private static void SeedUsers(BrewdudeDbIdentityContext identityContext, out string userId)
        {
            var brewdudeUser = new BrewdudeUser
            {
                UserName = UserName
            };

            identityContext.Users.Add(brewdudeUser);
            identityContext.SaveChangesAsync();

            userId = identityContext.Users.
                SingleOrDefault(u => string.Equals(u.UserName, UserName, StringComparison.CurrentCultureIgnoreCase))
                ?.Id;
        }
        
        private static void SeedUserBeers(BrewdudeDbContext context, string userId)
        {
            var userBeers = new[]
            {
                new Domain.Entities.UserBeers { UserId = userId, BeerId = 1 },
                new Domain.Entities.UserBeers { UserId = userId, BeerId = 3 }
            };
            
            context.UserBeers.AddRange(userBeers);
            context.SaveChanges();
        }

        private static void SeedUserBreweries(BrewdudeDbContext context, string userId)
        {
            var userBreweries = new[]
            {
                new Domain.Entities.UserBreweries { UserId = userId, BreweryId = 2 },
                new Domain.Entities.UserBreweries { UserId = userId, BreweryId = 3 },
                new Domain.Entities.UserBreweries { UserId = userId, BreweryId = 1 }
            };
            
            context.UserBreweries.AddRange(userBreweries);
            context.SaveChanges();
        }
    }
}