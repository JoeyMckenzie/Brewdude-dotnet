using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class GetBeersByUserIdQuery : IRequest<BrewdudeApiResponse<UserBeersViewModel>>
    {
        public GetBeersByUserIdQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}