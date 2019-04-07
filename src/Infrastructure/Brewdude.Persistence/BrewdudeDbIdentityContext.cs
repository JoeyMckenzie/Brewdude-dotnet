using Brewdude.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeDbIdentityContext : IdentityDbContext<BrewdudeUser>
    {
        public BrewdudeDbIdentityContext(DbContextOptions<BrewdudeDbIdentityContext> options)
            : base(options)
        {
        }
    }
}