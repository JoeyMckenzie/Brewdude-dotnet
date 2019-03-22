using System.Collections.Generic;

namespace Brewdude.Domain.ViewModels
{
    public class UserBreweriesViewModel
    {
        public IEnumerable<UserBreweryDto> Breweries { get; set; }
        public int UserId { get; set; }
        public bool CanEdit { get; set; }
    }
}