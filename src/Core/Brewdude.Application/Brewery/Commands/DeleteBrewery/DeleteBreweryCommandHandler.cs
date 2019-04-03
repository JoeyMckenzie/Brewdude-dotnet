using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    public class DeleteBreweryCommandHandler : IRequestHandler<DeleteBreweryCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<DeleteBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;

        public DeleteBreweryCommandHandler(BrewdudeDbContext context, ILogger<DeleteBreweryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(DeleteBreweryCommand request, CancellationToken cancellationToken)
        {
            var brewery = await _context.Breweries.FindAsync(request.BreweryId);

            if (brewery == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"Brewery [{request.BreweryId}] not found");

            _context.Breweries.Remove(brewery);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Successfully deleted brewery with ID [{request.BreweryId}]");

            return new BrewdudeApiResponse((int)HttpStatusCode.OK, BrewdudeResponseMessage.Deleted.GetDescription());
        }
    }
}