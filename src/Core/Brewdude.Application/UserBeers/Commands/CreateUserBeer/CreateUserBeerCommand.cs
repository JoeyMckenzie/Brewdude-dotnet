using Brewdude.Domain.Api;
using MediatR;

namespace Brewdude.Application.UserBeers.Commands.CreateUserBeer
{
    public class CreateUserBeerCommand : IRequest<BrewdudeApiResponse>
    {
        public string UserId { get; set; }
        public int BeerId { get; set; }
    }
}