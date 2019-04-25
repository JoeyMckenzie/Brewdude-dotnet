namespace Brewdude.Application.User.Commands.CreateUser
{
    using Domain.Api;
    using Domain.Entities;
    using Domain.ViewModels;
    using MediatR;

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