namespace Brewdude.Application.Beer.Queries.GetBeerByName
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBeerByNameQuery : IRequest<BrewdudeApiResponse<BeerListViewModel>>
    {
        public GetBeerByNameQuery(string beerName)
        {
            BeerName = beerName;
        }

        public string BeerName { get; }
    }
}