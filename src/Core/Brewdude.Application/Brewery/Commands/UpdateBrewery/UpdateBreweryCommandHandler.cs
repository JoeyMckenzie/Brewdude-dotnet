using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.UpdateBrewery
{
    public class UpdateBreweryCommandHandler : IRequestHandler<UpdateBreweryCommand>
    {
        private readonly BrewdudeDbContext _context;

        public UpdateBreweryCommandHandler(BrewdudeDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateBreweryCommand request, CancellationToken cancellationToken)
        {
            var breweryToUpdate = await _context.Breweries.FindAsync(request.BreweryId);
            
            if (breweryToUpdate == null)
                throw new BreweryNotFound($"Brewery [{request.BreweryId}] not found to update");

            breweryToUpdate.Name = request.Name;
            breweryToUpdate.Description = request.Description;
//            breweryToUpdate.City = request.City;
//            breweryToUpdate.State = request.State;
//            breweryToUpdate.StreetAddress = request.StreetAddress;
//            breweryToUpdate.ZipCode = request.ZipCode;
            breweryToUpdate.UpdatedAt = DateTime.UtcNow;
            breweryToUpdate.Website = string.IsNullOrWhiteSpace(request.Website) ? string.Empty : request.Website;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}