namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    using Domain.Api;
    using Domain.Entities;
    using MediatR;

    public class CreateBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public BeerStyle BeerStyle { get; set; }

        public int Ibu { get; set; }

        public double Abv { get; set; }

        public int BreweryId { get; set; }
    }
}