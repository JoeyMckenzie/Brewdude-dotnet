namespace Brewdude.Application.UserBeers.Commands.DeleteUserBeer
{
    using Domain.Api;
    using MediatR;

    public class DeleteUserBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public string UserId { get; set; }

        public int BeerId { get; set; }
    }
}