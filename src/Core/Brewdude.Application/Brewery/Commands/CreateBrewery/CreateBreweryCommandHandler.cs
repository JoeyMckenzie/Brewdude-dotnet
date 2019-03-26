using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common;
using Brewdude.Domain.Dtos;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, int>
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

        public async Task<int> Handle(CreateBreweryCommand request, CancellationToken cancellationToken)
        {
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

            return brewery.BreweryId;
        }
    }
}