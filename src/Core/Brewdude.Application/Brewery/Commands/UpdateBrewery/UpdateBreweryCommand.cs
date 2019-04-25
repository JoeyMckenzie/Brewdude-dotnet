namespace Brewdude.Application.Brewery.Commands.UpdateBrewery
{
    using Domain.Api;
    using Domain.Dtos;
    using MediatR;

    public class UpdateBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public UpdateBreweryCommand(int breweryId)
        {
            BreweryId = breweryId;
        }

        public int BreweryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public AddressDto AddressDto { get; set; }

        public string Website { get; set; }
    }
}