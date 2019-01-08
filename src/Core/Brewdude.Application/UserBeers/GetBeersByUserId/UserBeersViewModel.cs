using System.Collections.Generic;

namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class UserBeersViewModel
    {
        public IEnumerable<UserBeerDto> UserBeers { get; set; }
        public int UserId { get; set; }
        public bool CanEdit { get; set; }
    }
}