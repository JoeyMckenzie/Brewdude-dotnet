namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetAllBeersQuery : IRequest<BrewdudeApiResponse<BeerListViewModel>>
    {
    }
}