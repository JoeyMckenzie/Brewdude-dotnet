namespace Brewdude.Application.UserBreweries.Queries.GetBreweriesByUserId
{
    using Brewdude.Domain.Api;
    using Brewdude.Domain.ViewModels;
    using MediatR;

    public class GetBreweriesByUserIdQuery : IRequest<BrewdudeApiResponse<UserBreweryListViewModel>>
    {
        public GetBreweriesByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}