using System;
using System.Threading;
using System.Threading.Tasks;
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