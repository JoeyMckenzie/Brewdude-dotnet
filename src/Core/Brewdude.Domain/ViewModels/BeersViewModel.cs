using System.Collections.Generic;

namespace Brewdude.Domain.ViewModels
{
    public class BeersViewModel
    {
        public IEnumerable<BeersViewModel> BeersViewModels { get; set; }
        public bool CanEdit { get; set; }
    }
}