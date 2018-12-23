using Brewdude.Application.Beer.Queries.GetAllBeers;
using MediatR;

namespace Brewdude.Application.Beer.GetAllBeers.Queries
{
    public class GetAllBeersQuery : IRequest<BeerListViewModel>
    {
        
    }
}