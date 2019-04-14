using Brewdude.Domain.Api;
using MediatR;

namespace Brewdude.Application.UserBreweries.Commands
{
    public class CreateUserBreweryCommand : IRequest<BrewdudeApiResponse>
    {
        public string UserId { get; set; }
        public int BreweryId { get; set; }
    }
}