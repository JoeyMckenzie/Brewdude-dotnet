using System.Collections.Generic;
using Brewdude.Application.Brewery.Queries.GetBreweryById;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class BreweryViewModelList
    {
        public IEnumerable<BreweryViewModel> Breweries { get; set; }
        public bool CanEdit { get; set; }
    }
}