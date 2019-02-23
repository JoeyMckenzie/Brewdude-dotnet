using Brewdude.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeIdentityContextFactory : DesignTimeDbContextFactoryBase<BrewdudeIdentityContext>
    {
        protected override BrewdudeIdentityContext CreateNewInstance(DbContextOptions<BrewdudeIdentityContext> options)
        {
            return new BrewdudeIdentityContext(options);
        }
    }
}