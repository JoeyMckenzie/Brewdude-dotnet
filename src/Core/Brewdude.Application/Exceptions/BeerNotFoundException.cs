using System;

namespace Brewdude.Application.Exceptions
{
    public class BeerNotFoundException : Exception
    {
        public BeerNotFoundException()
            : base()
        {
        }

        public BeerNotFoundException(string message)
            : base(message)
        {
        }

        public BeerNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}