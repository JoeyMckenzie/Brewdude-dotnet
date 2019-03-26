using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<BrewdudeApiResponse<UserViewModel>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}