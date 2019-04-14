using System.Collections.Generic;
using System.Linq;
using Brewdude.Domain.Dtos;

namespace Brewdude.Domain.ViewModels
{
    public class UserBreweryListViewModel : BaseViewModel<BreweryViewModel>
    {
        public string UserId { get; set; }
    }
}