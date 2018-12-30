using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.DeleteBrewery
{
    public class DeleteBreweryCommandHandler : IRequestHandler<DeleteBreweryCommand>
    {
        private readonly BrewdudeDbContext _context;

        public DeleteBreweryCommandHandler(BrewdudeDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteBreweryCommand request, CancellationToken cancellationToken)
        {
            var brewery = await _context.Breweries.FindAsync(request.BreweryId);

            if (brewery == null)
                throw new BreweryNotFound($"Brewery [{request.BreweryId}] not found");

            _context.Breweries.Remove(brewery);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}