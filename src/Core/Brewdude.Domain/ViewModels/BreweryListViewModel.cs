namespace Brewdude.Domain.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class BreweryListViewModel
    {
        public IEnumerable<BreweryViewModel> Breweries { get; set; }

        public bool CanEdit { get; set; }

        public int Count => Breweries.Count();
    }
}