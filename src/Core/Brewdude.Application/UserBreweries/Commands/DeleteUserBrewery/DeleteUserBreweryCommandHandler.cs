namespace Brewdude.Application.UserBreweries.Commands.DeleteUserBrewery
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

    public class DeleteUserBreweryCommandHandler : IRequestHandler<DeleteUserBreweryCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<DeleteUserBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;

        public DeleteUserBreweryCommandHandler(BrewdudeDbContext context, ILogger<DeleteUserBreweryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(DeleteUserBreweryCommand request, CancellationToken cancellationToken)
        {
            // Validate the user brewery exists
            var existingUserBrewery = await _context.UserBreweries
                .Where(ub => string.Equals(request.UserId, ub.UserId, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefaultAsync(ub => ub.BreweryId == request.BreweryId, cancellationToken);

            if (existingUserBrewery == null || !string.Equals(request.UserId, existingUserBrewery.UserId, StringComparison.CurrentCultureIgnoreCase))
            {
                // Do not proceed if the user brewery is null, or the user IDs do not match
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"Brewery [{request.BreweryId}] was not found for user [{request.UserId}]");
            }

            _context.Remove(existingUserBrewery);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User brewery [{request.BreweryId}] successfully removed");

            return new BrewdudeApiResponse(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription());
        }
    }
}