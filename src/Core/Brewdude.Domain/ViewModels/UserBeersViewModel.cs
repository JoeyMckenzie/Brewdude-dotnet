using System.Collections.Generic;
using System.Linq;

namespace Brewdude.Domain.ViewModels
{
    public class UserBeersViewModel
    {
        public IEnumerable<UserBeerDto> UserBeers { get; set; }
        public string UserId { get; set; }
        public bool CanEdit { get; set; }
        public int Count => UserBeers.Count();
    }
}