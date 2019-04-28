namespace Brewdude.Application.UserBreweries.Commands.DeleteUserBrewery
{
    using Domain.Api;
    using MediatR;

    public class DeleteUserBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteUserBreweryCommand(int breweryId, string userId)
        {
            BreweryId = breweryId;
            UserId = userId;
        }

        public string UserId { get; set; }

        public int BreweryId { get; }
    }
}