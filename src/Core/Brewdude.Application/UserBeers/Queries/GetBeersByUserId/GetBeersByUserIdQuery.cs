using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.UserBeers.Queries.GetBeersByUserId
{
    public class GetBeersByUserIdQuery : IRequest<BrewdudeApiResponse<UserBeerListViewModel>>
    {
        public GetBeersByUserIdQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}