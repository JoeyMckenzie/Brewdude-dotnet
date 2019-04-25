namespace Brewdude.Application.UserBeers.Commands.CreateUserBeer
{
    using Domain.Api;
    using MediatR;

    public class CreateUserBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public string UserId { get; set; }

        public int BeerId { get; set; }
    }
}