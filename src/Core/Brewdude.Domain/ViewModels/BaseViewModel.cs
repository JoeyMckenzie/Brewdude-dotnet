using System;
using System.Collections.Generic;
using System.Linq;

namespace Brewdude.Domain.ViewModels
{
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