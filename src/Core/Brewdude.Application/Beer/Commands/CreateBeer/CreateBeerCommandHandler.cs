using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Common;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Beer.Commands.CreateBeer
{
    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<CreateBeerCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateBeerCommandHandler(BrewdudeDbContext context, IDateTime dateTime, ILogger<CreateBeerCommandHandler> logger)
        {
            _context = context;
            _dateTime = dateTime;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            // Validate beer to be added does not already exist
            var existingBeer = await _context.Beers.SingleOrDefaultAsync(b =>
                string.Equals(b.Name, request.Name, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            
            if (existingBeer != null)
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"Beer with name [{request.Name}] already exists");
            
            // Validate an existing brewery to add the beer
            var existingBrewery = await _context.Breweries.FindAsync(request.BreweryId);
            
            if (existingBrewery == null)
                _logger.LogWarning($"No brewery found for beer $[{request.Name}]");
                // throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BreweryNotFound, $"No brewery with ID [{request.BreweryId}] was found");
            
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
            
            return new BrewdudeApiResponse((int)HttpStatusCode.Created, BrewdudeResponseMessage.Created.GetDescription());
        }
    }
}