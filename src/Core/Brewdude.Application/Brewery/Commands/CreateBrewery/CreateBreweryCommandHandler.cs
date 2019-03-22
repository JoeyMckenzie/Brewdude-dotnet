using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Domain.Dtos;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Commands.CreateBrewery
{
    public class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, int>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBreweryCommandHandler> _logger;

        public CreateBreweryCommandHandler(BrewdudeDbContext context, IMapper mapper, ILogger<CreateBreweryCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateBreweryCommand request, CancellationToken cancellationToken)
        {
            var brewery = new Domain.Entities.Brewery
            {
                Name = request.Name,
                Description = request.Description,
                Address = _mapper.Map<Address>(request.AddressDto),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Website = string.IsNullOrWhiteSpace(request.Website) ? string.Empty : request.Website
            };

            _context.Add(brewery);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Brewery successfully created for brewer [{brewery.BreweryId}]");

            return brewery.BreweryId;
        }
    }
}