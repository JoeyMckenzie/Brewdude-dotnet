namespace Brewdude.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class BrewdudeUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}