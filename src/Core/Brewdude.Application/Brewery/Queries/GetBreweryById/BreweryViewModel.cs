using System.Collections.Generic;
using Brewdude.Application.Beer.Queries.GetBeerById;

namespace Brewdude.Application.Brewery.Queries.GetBreweryById
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