namespace Brewdude.Application.Beer.Commands.DeleteBeer
{
    using Domain.Api;
    using MediatR;

    public class DeleteBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteBeerCommand(int id)
        {
            BeerId = id;
        }

        public int BeerId { get; }
    }
}