using MediatR;

namespace Brewdude.Application.Beer.Commands.DeleteBeer
{
    public class DeleteBeerCommand : IRequest
    {
        public DeleteBeerCommand(int id)
        {
            BeerId = id;
        }
        
        public int BeerId { get; }
    }
}