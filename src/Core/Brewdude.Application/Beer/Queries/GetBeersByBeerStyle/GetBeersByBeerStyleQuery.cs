namespace Brewdude.Application.Beer.Queries.GetBeersByBeerStyle
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBeersByBeerStyleQuery : IRequest<BrewdudeApiResponse<BeerListViewModel>>
    {
        public GetBeersByBeerStyleQuery(string beerStyle)
        {
            BeerStyle = beerStyle;
        }

        public string BeerStyle { get; }
    }
}