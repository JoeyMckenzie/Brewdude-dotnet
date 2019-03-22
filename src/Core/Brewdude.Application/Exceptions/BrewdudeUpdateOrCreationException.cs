using System;

namespace Brewdude.Application.Exceptions
{
    public class BrewdudeUpdateOrCreationException : Exception
    {
        public BrewdudeUpdateOrCreationException()
            : base()
        {
        }

        public BrewdudeUpdateOrCreationException(string message)
            : base(message)
        {
        }

        public BrewdudeUpdateOrCreationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}