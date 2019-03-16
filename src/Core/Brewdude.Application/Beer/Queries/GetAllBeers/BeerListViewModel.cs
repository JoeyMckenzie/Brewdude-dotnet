using System.Collections.Generic;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;

namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    public class BeerListViewModel
    {
        public IEnumerable<BeerDto> Beers { get; set; }
        public int BeersResultLength { get; set; }
        public bool CanEdit { get; set; } = true;
    }
}