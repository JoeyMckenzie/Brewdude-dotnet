namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    using Domain.Api;
    using Domain.Dtos;
    using MediatR;

    public class CreateBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public AddressDto AddressDto { get; set;  }

        public string Website { get; set; }
    }
}