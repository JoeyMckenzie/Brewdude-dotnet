namespace Brewdude.Application.UserBeers.Commands.DeleteUserBeer
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Domain.Api;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class DeleteUserBeerCommandHandler : IRequestHandler<DeleteUserBeerCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<DeleteUserBeerCommand> _logger;
        private readonly BrewdudeDbContext _context;

        public DeleteUserBeerCommandHandler(ILogger<DeleteUserBeerCommand> logger, BrewdudeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<BrewdudeApiResponse> Handle(DeleteUserBeerCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the user beer
            var existingUserBeer = await _context.UserBeers.FindAsync(request.BeerId);

            if (existingUserBeer == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"User beer [{request.BeerId}] was not found for user [{request.UserId}]");
            }

            _context.UserBeers.Remove(existingUserBeer);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User beer [{request.BeerId}] successfully delete user [{request.UserId}]");

            return new BrewdudeApiResponse(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription());
        }
    }
}