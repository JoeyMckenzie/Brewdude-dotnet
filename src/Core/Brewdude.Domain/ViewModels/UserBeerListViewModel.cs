using System.Collections.Generic;
using System.Linq;

namespace Brewdude.Domain.ViewModels
{
    public class UserBeerListViewModel : BaseViewModel<BeerViewModel>
    {
        public string UserId { get; set; }
    }
}