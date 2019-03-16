using MediatR;

namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    public class GetAllBeersQuery : IRequest<BeerListViewModel>
    {
    }
}