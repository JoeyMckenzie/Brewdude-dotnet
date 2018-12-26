using Brewdude.Application.User.Commands.Models;
using MediatR;

namespace Brewdude.Application.User.Queries.GetUserById
{
    public class GetUserByIdCommand : IRequest<UserViewModel>
    {
        public GetUserByIdCommand(int userId)
        {
            UserId = userId;
        }
        
        public int UserId { get; }
    }
}