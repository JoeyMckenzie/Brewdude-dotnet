using System.Collections.Generic;
using Brewdude.Domain.Dtos;

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
        public AddressDto Address { get; set; }
        public ICollection<BeerViewModel> Beers { get; set; }
    }
}