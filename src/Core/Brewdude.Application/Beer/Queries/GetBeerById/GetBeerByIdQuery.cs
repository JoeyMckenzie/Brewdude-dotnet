using MediatR;

namespace Brewdude.Application.Beer.Queries.GetBeerById
{
    public class GetBeerByIdQuery : IRequest<BeerViewModel>
    {
        public GetBeerByIdQuery(int beerId)
        {
            BeerId = beerId;
        }

        public int BeerId { get; }
    }
}