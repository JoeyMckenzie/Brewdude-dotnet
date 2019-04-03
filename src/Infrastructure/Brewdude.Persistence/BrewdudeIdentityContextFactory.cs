using Brewdude.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeIdentityContextFactory : DesignTimeDbContextFactoryBase<BrewdudeDbIdentityContext>
    {
        protected override BrewdudeDbIdentityContext CreateNewInstance(DbContextOptions<BrewdudeDbIdentityContext> options)
        {
            return new BrewdudeDbIdentityContext(options);
        }
    }
}