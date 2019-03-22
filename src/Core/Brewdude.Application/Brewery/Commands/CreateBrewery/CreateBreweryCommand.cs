using Brewdude.Domain.Dtos;
using Brewdude.Domain.Entities;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
//        public string StreetAddress { get; set; }
//        public string City { get; set; }
//        public string State { get; set; }
//        public int ZipCode { get; set; }
        public AddressDto AddressDto { get; set; }
        public string Website { get; set; }
    }
}