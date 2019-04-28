namespace Brewdude.Application.Brewery.Queries.GetBreweryByName
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBreweryByNameQuery : IRequest<BrewdudeApiResponse<BreweryListViewModel>>
    {
        public GetBreweryByNameQuery(string breweryName)
        {
            BreweryName = breweryName;
        }

        public string BreweryName { get; }
    }
}