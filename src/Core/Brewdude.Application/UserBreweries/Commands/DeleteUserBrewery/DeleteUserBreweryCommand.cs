namespace Brewdude.Application.UserBreweries.Commands.DeleteUserBrewery
{
    using Domain.Api;
    using MediatR;

    public class DeleteUserBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public DeleteUserBreweryCommand(int userBreweryId)
        {
            UserBreweryId = userBreweryId;
        }

        public int UserBreweryId { get; }
    }
}