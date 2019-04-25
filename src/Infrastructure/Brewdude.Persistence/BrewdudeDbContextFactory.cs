namespace Brewdude.Persistence
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class BrewdudeDbContextFactory : DesignTimeDbContextFactoryBase<BrewdudeDbContext>
    {
        protected override BrewdudeDbContext CreateNewInstance(DbContextOptions<BrewdudeDbContext> options)
        {
            return new BrewdudeDbContext(options);
        }
    }
}