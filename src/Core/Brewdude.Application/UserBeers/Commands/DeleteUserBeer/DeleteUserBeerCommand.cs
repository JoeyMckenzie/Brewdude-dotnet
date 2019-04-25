namespace Brewdude.Application.UserBeers.Commands.DeleteUserBeer
{
    using Domain.Api;
    using MediatR;

    public class DeleteUserBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteUserBeerCommand(int beerId, string userId)
        {
            BeerId = beerId;
            UserId = userId;
        }

        public string UserId { get; }

        public int BeerId { get; }
    }
}