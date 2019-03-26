using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    public class DeleteBreweryCommandHandler : IRequestHandler<DeleteBreweryCommand>
    {
        private readonly ILogger<DeleteBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;

        public DeleteBreweryCommandHandler(BrewdudeDbContext context, ILogger<DeleteBreweryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBreweryCommand request, CancellationToken cancellationToken)
        {
            var brewery = await _context.Breweries.FindAsync(request.BreweryId);

            if (brewery == null)
            {
                _logger.LogError($"Delete brewery with ID [{request.BreweryId}] failed, no brewery was found");
                throw new BreweryNotFound($"Brewery [{request.BreweryId}] not found");
            }

            _context.Breweries.Remove(brewery);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Successfully deleted brewery with ID [{request.BreweryId}]");

            return Unit.Value;
        }
    }
}