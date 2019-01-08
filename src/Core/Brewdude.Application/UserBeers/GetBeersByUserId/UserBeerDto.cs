namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class UserBeerDto
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Abv { get; set; }
        public int Ibu { get; set; }
        public int BreweryId { get; set; }
    }
}