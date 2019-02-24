using Brewdude.Application.User.Models;
using Brewdude.Domain.Entities;
using MediatR;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}