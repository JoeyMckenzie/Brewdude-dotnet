namespace Brewdude.Application.UserBeers.Queries.GetBeersByUserId
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBeersByUserIdQuery : IRequest<BrewdudeApiResponse<UserBeerListViewModel>>
    {
        public GetBeersByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}