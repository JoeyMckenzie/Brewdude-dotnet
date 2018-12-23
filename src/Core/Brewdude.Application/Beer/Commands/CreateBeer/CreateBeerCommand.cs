using Brewdude.Domain.Entities;
using MediatR;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BeerStyle BeerStyle { get; set; }
        public byte Ibu { get; set; }
        public double Abv { get; set; }
        public int BreweryId { get; set; }
    }
}