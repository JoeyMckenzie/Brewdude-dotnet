namespace Brewdude.Domain.Dtos
{
    /// <summary>
    /// User beer data transfer object used within the application layer.
    /// </summary>
    public class UserBeerDto
    {
        /// <summary>
        /// Gets or sets the beer ID primary key.
        /// </summary>
        public int BeerId { get; set; }

        /// <summary>
        /// Gets or sets the beer name pertaining to the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets beer description pertaining to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ABV of the beer entity pertaining to the user.
        /// </summary>
        public double Abv { get; set; }

        /// <summary>
        /// Gets or sets the IBU or the beer entity pertaining to the user.
        /// </summary>
        public int Ibu { get; set; }

        /// <summary>
        /// Gets or sets the brewery ID of the beer pertaining to the user.
        /// </summary>
        public int BreweryId { get; set; }
    }
}