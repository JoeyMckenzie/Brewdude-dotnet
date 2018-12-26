using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.Beer.Commands.DeleteBeer
{
    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand>
    {
        private readonly BrewdudeDbContext _context;

        public DeleteBeerCommandHandler(BrewdudeDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FindAsync(request.BeerId);
            
            if (beer == null)
                throw new BeerNotFoundException($"Beer with ID [{request.BeerId}] not found");

            _context.Remove(beer);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}