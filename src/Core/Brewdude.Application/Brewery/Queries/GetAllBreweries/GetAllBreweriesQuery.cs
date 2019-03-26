using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using MediatR;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class GetAllBreweriesQuery : IRequest<BrewdudeApiResponse<BreweryViewModelList>>
    {
    }
}