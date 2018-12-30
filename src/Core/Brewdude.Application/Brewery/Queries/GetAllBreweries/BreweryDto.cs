using System.Collections.Generic;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class BreweryDto
    {
        public BreweryDto()
        {
            Beers = new HashSet<BeerDto>();
        }

        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public ICollection<BeerDto> Beers { get; set; }
    }
}