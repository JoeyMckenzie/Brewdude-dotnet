using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brewdude.Domain.Entities
{
    public class Brewery : BaseEntity
    {
        public Brewery()
        {
            Beers = new HashSet<Beer>();
        }
        
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Website { get; set; }

        public ICollection<Beer> Beers { get; private set; }
    }
}