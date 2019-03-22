using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class GetAllBreweriesQuery : IRequest<BreweryViewModelList>
    {
        
    }
}