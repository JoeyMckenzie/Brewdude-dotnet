using Brewdude.Domain.Api;
using MediatR;

namespace Brewdude.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteUserCommand(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; set; }
    }
}