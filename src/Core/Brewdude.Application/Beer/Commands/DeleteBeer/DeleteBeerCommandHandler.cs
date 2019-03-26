using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Beer.Commands.DeleteBeer
{
    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, BrewdudeApiResponse>
    {
        private readonly BrewdudeDbContext _context;
        private readonly ILogger<DeleteBeerCommandHandler> _logger;

        public DeleteBeerCommandHandler(BrewdudeDbContext context, ILogger<DeleteBeerCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FindAsync(request.BeerId);
            
            if (beer == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"Beer with ID [{request.BeerId}] not found");

            _context.Remove(beer);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully deleted beer: @{beer}", beer);

            return new BrewdudeApiResponse((int)HttpStatusCode.OK, $"Beer [{beer.Name}] has been successfully deleted");
        }
    }
}