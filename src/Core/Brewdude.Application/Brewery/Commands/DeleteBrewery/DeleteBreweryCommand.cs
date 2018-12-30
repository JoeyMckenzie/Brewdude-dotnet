using MediatR;

namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    public class DeleteBreweryCommand : IRequest
    {
        public DeleteBreweryCommand(int breweryId)
        {
            BreweryId = breweryId;
        }
        
        public int BreweryId { get; }
    }
}