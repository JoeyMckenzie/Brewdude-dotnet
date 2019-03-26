using Brewdude.Domain.Api;
using MediatR;

namespace Brewdude.Application.Beer.Commands.DeleteBeer
{
    public class DeleteBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteBeerCommand(int id)
        {
            BeerId = id;
        }
        
        public int BeerId { get; }
    }
}