using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.User.Queries.GetUserByUsername
{
    public class GetUserByUsernameCommand : IRequest<UserViewModel>
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