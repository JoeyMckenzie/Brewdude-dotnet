using System;

namespace Brewdude.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message)
            : base(message)
        {
        }

        public UserNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}