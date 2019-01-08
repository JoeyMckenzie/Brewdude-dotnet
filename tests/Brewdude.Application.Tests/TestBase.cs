using Brewdude.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Tests
{
    public class TestBase
    {
        public BrewdudeDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<BrewdudeDbContext>()
                .UseInMemoryDatabase("Brewdude.Application.Tests.Db");
            
            var context = new BrewdudeDbContext(builder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}