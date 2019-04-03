using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Brewery.Queries.GetBreweryById
{
    public class GetBreweryByIdQuery : IRequest<BrewdudeApiResponse<BreweryViewModel>>
    {
        public GetBreweryByIdQuery(int id)
        {
            BreweryId = id;
        }
        
        public int BreweryId { get; }
    }
}