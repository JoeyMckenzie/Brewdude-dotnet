namespace Brewdude.Domain.Dtos
{
    using System.Collections.Generic;

    /// <summary>
    /// User brewery data transfer object used within the application layer.
    /// </summary>
    public abstract class UserBreweryDto
    {
        /// <summary>
        /// Gets or sets the brewery ID pertaining to the user.
        /// </summary>
        public int BreweryId { get; set; }

        /// <summary>
        /// Gets or sets the brewery name pertaining to the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the brewery description pertaining to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the brewery address data transfer object pertaining to the user.
        /// </summary>
        public AddressDto Address { get; set; }

        /// <summary>
        /// Gets or sets the enumerable beers data transfer objects pertaining to the user.
        /// </summary>
        public IEnumerable<BeerDto> Beers { get; set; }
    }
}