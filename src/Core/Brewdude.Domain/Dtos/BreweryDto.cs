namespace Brewdude.Domain.Dtos
{
    using System.Collections.Generic;

    public class BreweryDto
    {
        public int BreweryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public AddressDto Address { get; set; }

        public ICollection<BeerDto> Beers { get; set; }
    }
}