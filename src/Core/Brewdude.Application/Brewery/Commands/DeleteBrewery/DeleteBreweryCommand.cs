namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    using Domain.Api;
    using MediatR;

    public class DeleteBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteBreweryCommand(int breweryId)
        {
            BreweryId = breweryId;
        }

        public int BreweryId { get; }
    }
}