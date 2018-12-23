using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, int>
    {
        private readonly BrewdudeDbContext _context;

        public CreateBeerCommandHandler(BrewdudeDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            var beer = new Domain.Entities.Beer
            {
                Name = request.Name,
                Abv = request.Abv,
                Description = request.Description,
                Ibu = request.Ibu,
                BeerStyle = request.BeerStyle,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BreweryId = request.BreweryId
            };

            _context.Beers.Add(beer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return beer.BeerId;
        }
    }
}