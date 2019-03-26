using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    public class GetAllBeersQuery : IRequest<BrewdudeApiResponse<BeerListViewModel>>
    {
    }
}