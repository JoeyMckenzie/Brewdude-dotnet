using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.UserBreweries.Queries.GetBreweriesByUserId
{
    public class GetBreweriesByUserIdQuery : IRequest<BrewdudeApiResponse<UserBreweryListViewModel>>
    {
        public GetBreweriesByUserIdQuery(string userId)
        {
            UserId = userId;
        }
        
        public string UserId { get; }
    }
}