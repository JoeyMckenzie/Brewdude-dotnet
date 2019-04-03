using Brewdude.Domain.Api;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    public class DeleteBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteBreweryCommand(int breweryId)
        {
            BreweryId = breweryId;
        }
        
        public int BreweryId { get; }
    }
}