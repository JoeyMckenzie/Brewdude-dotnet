using Brewdude.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Persistence
{
    public class BrewdudeDbContextFactory : DesignTimeDbContextFactoryBase<BrewdudeDbContext>
    {
        protected override BrewdudeDbContext CreateNewInstance(DbContextOptions<BrewdudeDbContext> options)
        {
            return new BrewdudeDbContext(options);
        }
    }
}