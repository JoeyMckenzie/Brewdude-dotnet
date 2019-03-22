using System.Collections.Generic;

namespace Brewdude.Domain.ViewModels
{
    public class BreweryViewModelList
    {
        public IEnumerable<BreweryViewModel> Breweries { get; set; }
        public bool CanEdit { get; set; }
    }
}