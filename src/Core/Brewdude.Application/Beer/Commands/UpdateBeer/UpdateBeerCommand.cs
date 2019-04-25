namespace Brewdude.Application.Beer.Commands.UpdateBeer
{
    using Domain.Api;
    using Domain.Entities;
    using MediatR;

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