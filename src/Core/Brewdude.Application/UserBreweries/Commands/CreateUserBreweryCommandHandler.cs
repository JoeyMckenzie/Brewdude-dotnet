using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.UserBreweries.Commands
{
    public class CreateUserBreweryCommandHandler : IRequestHandler<CreateUserBreweryCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<CreateUserBreweryCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserBreweryCommandHandler(ILogger<CreateUserBreweryCommandHandler> logger, BrewdudeDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse> Handle(CreateUserBreweryCommand request, CancellationToken cancellationToken)
        {
            // Validate the brewery exists from the request
            var existingBrewery = await _context.Breweries.FindAsync(request.BreweryId);
            
            if (existingBrewery == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"brewery with ID [{request.BreweryId}] was not added to user ID [{request.UserId}], brewery does not exist");
            
            var existingUserBreweries = await _context.UserBreweries
                .Where(ub => ub.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            
            // Validate the request brewery to add does not already exist for the user
            if (existingUserBreweries.Exists(ub => ub.BreweryId == request.BreweryId))
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"User brewery [{request.UserId}] already contains brewery [{request.BreweryId}]");

            // Map to user brewery entity and add to context
            var userBeer = _mapper.Map<UserBrewery>(request);
            await _context.AddAsync(userBeer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User [{request.UserId}] has added brewery [{request.BreweryId}] successfully");

            return new BrewdudeApiResponse((int)HttpStatusCode.Created,
                BrewdudeResponseMessage.Created.GetDescription());
        }
    }
}