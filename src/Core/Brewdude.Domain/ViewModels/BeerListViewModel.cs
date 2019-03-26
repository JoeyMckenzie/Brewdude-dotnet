using System.Collections.Generic;
using Brewdude.Domain.Dtos;

namespace Brewdude.Domain.ViewModels
{
    public class BeerListViewModel
    {
        public IEnumerable<BeerDto> Beers { get; set; }
        public bool CanEdit { get; set; } = true;
    }
}