using Brewdude.Domain.Entities;

namespace Brewdude.Application.Beer.Queries.GetBeerById
{
    public class BeerViewModel
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BeerStyle BeerStyle { get; set; }
        public byte Ibu { get; set; }
        public double Abv { get; set; }
        public int BreweryId { get; set; }
        public bool CanEdit { get; set; } = true;
    }
}