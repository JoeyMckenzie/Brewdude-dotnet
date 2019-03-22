using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Brewery.Queries.GetBreweryById
{
    public class GetBreweryByIdQuery : IRequest<BreweryViewModel>
    {
        public GetBreweryByIdQuery(int id)
        {
            BreweryId = id;
        }
        
        public int BreweryId { get; }
    }
}