namespace Brewdude.Application.Beer.Queries.GetBeerById
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBeerByIdQuery : IRequest<BrewdudeApiResponse<BeerViewModel>>
    {
        public GetBeerByIdQuery(int beerId)
        {
            BeerId = beerId;
        }

        public int BeerId { get; }
    }
}