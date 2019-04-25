namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetAllBreweriesQuery : IRequest<BrewdudeApiResponse<BreweryListViewModel>>
    {
    }
}