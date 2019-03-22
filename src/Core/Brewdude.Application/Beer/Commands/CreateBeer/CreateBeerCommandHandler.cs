using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Common;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, int>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateBeerCommandHandler(BrewdudeDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<int> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            // Validate beer to be added does not already exist
            var existingBeer = await _context.Beers.FirstAsync(b =>
                string.Equals(b.Name, request.Name, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            
            if (existingBeer != null)
                throw new BrewdudeUpdateOrCreationException($"Beer with name [{request.Name}] already exists");
            
            var beer = new Domain.Entities.Beer
            {
                Name = request.Name,
                Abv = request.Abv,
                Description = request.Description,
                Ibu = request.Ibu,
                BeerStyle = request.BeerStyle,
                CreatedAt = _dateTime.Now,
                UpdatedAt = _dateTime.Now,
                BreweryId = request.BreweryId
            };

            _context.Beers.Add(beer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return beer.BeerId;
        }
    }
}