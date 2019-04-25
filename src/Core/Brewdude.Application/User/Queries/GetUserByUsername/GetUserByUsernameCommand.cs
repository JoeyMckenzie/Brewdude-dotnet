namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetUserByUsernameCommand : IRequest<BrewdudeApiResponse<UserViewModel>>
    {
        public GetUserByUsernameCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}