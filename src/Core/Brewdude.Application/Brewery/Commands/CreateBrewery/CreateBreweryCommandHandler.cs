using System;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, int>
    {
        private readonly BrewdudeDbContext _context;

        public CreateBreweryCommandHandler(BrewdudeDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateBreweryCommand request, CancellationToken cancellationToken)
        {
            var brewery = new Domain.Entities.Brewery
            {
                Name = request.Name,
                Description = request.Description,
                City = request.City,
                State = request.State,
                StreetAddress = request.StreetAddress,
                ZipCode = request.ZipCode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Add(brewery);
            await _context.SaveChangesAsync(cancellationToken);

            return brewery.BreweryId;
        }
    }
}