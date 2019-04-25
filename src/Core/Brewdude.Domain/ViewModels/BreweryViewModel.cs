namespace Brewdude.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;

    /// <summary>
    /// Brewery view model used within the application layer.
    /// </summary>
    public abstract class BreweryViewModel : BaseViewModel
    {
        protected BreweryViewModel()
        {
            Beers = new HashSet<BeerViewModel>();
        }

        /// <summary>
        /// Gets or sets the brewery ID associated with the primary key in the database.
        /// </summary>
        public int BreweryId { get; set; }

        /// <summary>
        /// Gets or sets the brewery name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the brewery short description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the brewery associated website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the address data transfer object.
        /// </summary>
        public AddressDto Address { get; set; }

        /// <summary>
        /// Gets or sets the beer enumerable associated with the given brewery.
        /// </summary>
        public ICollection<BeerViewModel> Beers { get; set; }

        /// <summary>
        /// Gets the number of beers returned from the associated brewery.
        /// </summary>
        public int NumberOfBeers => Beers.Count;
    }
}