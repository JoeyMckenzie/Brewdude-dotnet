using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.UpdateBrewery
{
    public class UpdateBreweryCommandHandler : IRequestHandler<UpdateBreweryCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<UpdateBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public UpdateBreweryCommandHandler(BrewdudeDbContext context, IMapper mapper, ILogger<UpdateBreweryCommandHandler> logger, IDateTime dateTime)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _dateTime = dateTime;
        }

        public async Task<BrewdudeApiResponse> Handle(UpdateBreweryCommand request, CancellationToken cancellationToken)
        {
            var breweryToUpdate = await _context.Breweries.FindAsync(request.BreweryId);

            if (breweryToUpdate == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"Brewery [{request.BreweryId}] not found to update");

            // Update brewery values
            breweryToUpdate.Name = request.Name;
            breweryToUpdate.Description = request.Description;
            breweryToUpdate.Address = _mapper.Map<Address>(request.AddressDto);
            breweryToUpdate.UpdatedAt = _dateTime.Now;
            breweryToUpdate.Website = string.IsNullOrWhiteSpace(request.Website) ? string.Empty : request.Website;
            
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Successfully updated brewery with ID [{request.BreweryId}]");

            return new BrewdudeApiResponse((int)HttpStatusCode.OK, BrewdudeResponseMessage.Success.GetDescription());
        }
    }
}