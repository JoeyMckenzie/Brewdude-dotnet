using System.Collections.Generic;

namespace Brewdude.Domain.ViewModels
{
    public class BreweryViewModel
    {
        public BreweryViewModel()
        {
            Beers = new HashSet<BeerViewModel>();
        }

        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public ICollection<BeerViewModel> Beers { get; set; }
    }
}