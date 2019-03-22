using System.Collections.Generic;

namespace Brewdude.Domain.Entities.IterableEntities
{
    public class IterableEntity<T>
    {
        public IterableEntity()
        {
            Results = new List<T>();
        }
        
        public IEnumerable<T> Results { get; set; }
    }
}