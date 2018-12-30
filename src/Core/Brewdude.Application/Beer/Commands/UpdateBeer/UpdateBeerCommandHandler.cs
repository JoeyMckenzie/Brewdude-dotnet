using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Beer.Commands.UpdateBeer
{
    public class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, Unit>
    {
        private readonly BrewdudeDbContext _context;
        private readonly ILogger<UpdateBeerCommandHandler> _logger;

        public UpdateBeerCommandHandler(BrewdudeDbContext context, ILogger<UpdateBeerCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            var beerToUpdate = await _context.Beers.FindAsync(request.BeerId);
            
            if (beerToUpdate == null)
                throw new BeerNotFoundException($"Beer with ID [{request.BeerId}] not found");

            beerToUpdate.Name = request.Name;
            beerToUpdate.Description = request.Description;
            beerToUpdate.BeerStyle = request.BeerStyle;
            beerToUpdate.Ibu = request.Ibu;
            beerToUpdate.Abv = request.Abv;
            beerToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Beer [{0}] updated successfully", beerToUpdate.BeerId);
            return Unit.Value;
        }
    }
}