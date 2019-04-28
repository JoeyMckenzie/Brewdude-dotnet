namespace Brewdude.Application.Brewery.Queries.GetBreweriesByState
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBreweriesByStateQuery : IRequest<BrewdudeApiResponse<BreweryListViewModel>>
    {
        public GetBreweriesByStateQuery(string state)
        {
            State = state;
        }

        public string State { get; }
    }
}