using Brewdude.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Persistence
{
    public class BrewdudeIdentityContext : IdentityDbContext<BrewdudeUser>
    {
        public BrewdudeIdentityContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}