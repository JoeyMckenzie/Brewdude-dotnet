namespace Brewdude.Application.UserBreweries.Commands.CreateUserBrewery
{
    using Domain.Api;
    using MediatR;

    public class CreateUserBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public string UserId { get; set; }

        public int BreweryId { get; set; }
    }
}