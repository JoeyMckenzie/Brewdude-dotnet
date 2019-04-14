namespace Brewdude.Domain.ViewModels
{
    public class BeerViewModel : BaseViewModel
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BeerStyle { get; set; }
        public int Ibu { get; set; }
        public double Abv { get; set; }
        public int BreweryId { get; set; }
    }
}