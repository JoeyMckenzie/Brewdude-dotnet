using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using MediatR;

namespace Brewdude.Application.Beer.Commands.UpdateBeer
{
    public class UpdateBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public UpdateBeerCommand(int beerId)
        {
            BeerId = beerId;
        }
        
        public int BeerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BeerStyle BeerStyle { get; set; }
        public int Ibu { get; set; }
        public double Abv { get; set; }
    }
}