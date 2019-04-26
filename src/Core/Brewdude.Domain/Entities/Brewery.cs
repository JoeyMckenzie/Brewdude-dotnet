namespace Brewdude.Domain.Entities
{
    using System.Collections.Generic;

    public class Brewery : BaseEntity
    {
        public Brewery()
        {
            Beers = new HashSet<Beer>();
        }

        public int BreweryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }

        public string Website { get; set; }

        public ICollection<Beer> Beers { get; }
    }
}