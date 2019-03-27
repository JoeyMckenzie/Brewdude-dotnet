using Brewdude.Domain.Api;
using Brewdude.Domain.Dtos;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto AddressDto { get; set; }
        public string Website { get; set; }
    }
}