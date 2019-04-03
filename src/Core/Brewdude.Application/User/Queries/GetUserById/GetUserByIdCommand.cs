using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.User.Queries.GetUserById
{
    public class GetUserByIdCommand : IRequest<BrewdudeApiResponse<UserViewModel>>
    {
        public GetUserByIdCommand(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}