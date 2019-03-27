using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Dtos;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<CreateBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public CreateBreweryCommandHandler(BrewdudeDbContext context, IMapper mapper, ILogger<CreateBreweryCommandHandler> logger, IDateTime dateTime)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _dateTime = dateTime;
        }

        public async Task<BrewdudeApiResponse> Handle(CreateBreweryCommand request, CancellationToken cancellationToken)
        {
            // Validate brewery does not already exist
            var existingBrewery = await _context.Breweries.SingleOrDefaultAsync(b =>
                string.Equals(b.Name, request.Name, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            
            if (existingBrewery != null)
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"Brewery with name [{request.Name}] already exists");
            
            var brewery = new Domain.Entities.Brewery
            {
                Name = request.Name,
                Description = request.Description,
                Address = _mapper.Map<Address>(request.AddressDto),
                CreatedAt = _dateTime.Now,
                UpdatedAt = _dateTime.Now,
                Website = string.IsNullOrWhiteSpace(request.Website) ? string.Empty : request.Website
            };

            _context.Add(brewery);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Brewery successfully created for brewery with ID [{brewery.BreweryId}]");

            return new BrewdudeApiResponse((int)HttpStatusCode.Created, BrewdudeResponseMessage.Created.GetDescription());
        }
    }
}