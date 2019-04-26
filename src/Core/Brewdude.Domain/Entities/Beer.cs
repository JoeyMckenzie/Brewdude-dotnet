namespace Brewdude.Domain.Entities
{
    public class Beer : BaseEntity
    {
        public int BeerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public BeerStyle BeerStyle { get; set; }

        public int Ibu { get; set; }

        public double Abv { get; set; }

        public int? BreweryId { get; set; }

        public virtual Brewery Brewery { get; set; }
    }
}