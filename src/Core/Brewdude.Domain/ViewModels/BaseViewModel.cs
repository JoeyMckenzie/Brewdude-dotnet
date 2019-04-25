namespace Brewdude.Domain.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class BaseViewModel
    {
        public bool CanEdit { get; set; }
    }

    public class BaseViewModel<T> : BaseViewModel
    {
        public IEnumerable<T> Results { get; set; }

        public int Count => Results.Count();
    }
}