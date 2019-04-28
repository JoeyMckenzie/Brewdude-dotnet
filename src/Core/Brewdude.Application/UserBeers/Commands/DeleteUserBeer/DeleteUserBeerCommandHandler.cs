namespace Brewdude.Application.UserBeers.Commands.DeleteUserBeer
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Domain.Api;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
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
            // Validate the existing user beer
            var existingUserBeer = await _context.UserBeers
                .Where(ub => string.Equals(request.UserId, ub.UserId, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefaultAsync(ub => ub.BeerId == request.BeerId, cancellationToken);

            if (existingUserBeer == null || !string.Equals(request.UserId, existingUserBeer.UserId, StringComparison.CurrentCultureIgnoreCase))
            {
                // Do not proceed if the user beer is null, or the user IDs do not match
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