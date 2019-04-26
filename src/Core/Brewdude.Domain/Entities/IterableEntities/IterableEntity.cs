namespace Brewdude.Domain.Entities.IterableEntities
{
    using System.Collections.Generic;
    using System.Linq;

    public class IterableEntity<T>
    {
        public IterableEntity()
        {
            Results = new List<T>();
        }

        public IEnumerable<T> Results { get; set; }

        public int Count => Results.Count();
    }
}