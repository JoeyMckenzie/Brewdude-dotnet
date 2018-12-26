using System;

namespace Brewdude.Application.Exceptions
{
    public class UserCreationException : Exception
    {
        public UserCreationException()
        {
        }

        public UserCreationException(string message)
            : base(message)
        {
        }

        public UserCreationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}