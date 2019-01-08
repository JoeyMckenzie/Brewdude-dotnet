using System.Collections.Generic;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;

namespace Brewdude.Application.UserBreweries.GetBreweriesByUserId
{
    public class UserBreweryDto
    {
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public IEnumerable<BeerDto> Beers { get; set; }
    }
}