using Microsoft.AspNetCore.Identity;

namespace Brewdude.Domain.Entities
{
    public class BrewdudeUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}