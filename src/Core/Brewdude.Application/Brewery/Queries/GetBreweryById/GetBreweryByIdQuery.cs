namespace Brewdude.Application.Brewery.Queries.GetBreweryById
{
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;

    public class GetBreweryByIdQuery : IRequest<BrewdudeApiResponse<BreweryViewModel>>
    {
        public GetBreweryByIdQuery(int id)
        {
            BreweryId = id;
        }

        public int BreweryId { get; }
    }
}