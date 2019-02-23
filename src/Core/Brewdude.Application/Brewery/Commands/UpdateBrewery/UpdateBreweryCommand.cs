using MediatR;

namespace Brewdude.Application.Brewery.Commands.UpdateBrewery
{
    public class UpdateBreweryCommand : IRequest
    {
        public UpdateBreweryCommand(int breweryId)
        {
            BreweryId = breweryId;
        }

        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Website { get; set; }
    }
}