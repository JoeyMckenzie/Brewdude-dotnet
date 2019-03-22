namespace Brewdude.Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int BreweryId { get; set; }
        public Brewery Brewery { get; set; }
    }
}