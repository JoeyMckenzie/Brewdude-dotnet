using System;

namespace Brewdude.Application.Exceptions
{
    public class BreweryNotFound : Exception
    {
        public BreweryNotFound()
        {
        }

        public BreweryNotFound(string message)
            : base(message)
        {
        }

        public BreweryNotFound(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}