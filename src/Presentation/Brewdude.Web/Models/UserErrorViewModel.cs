using System;

namespace Brewdude.Web.Models
{
    public class UserErrorViewModel
    {
        public UserErrorViewModel(string message)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            Message = message;
        }

        public Guid Id { get; set; }
        public DateTime TimeStamp { get; private set; }
        public string Message { get; private set; }
    }
}