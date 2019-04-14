using System.Collections.Generic;

namespace Brewdude.Domain.Dtos
{
    public class UserBreweryDto
    {
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<BeerDto> Beers { get; set; }
    }
}