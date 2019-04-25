namespace Brewdude.Application.UserBeers.Commands.CreateUserBeer
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Extensions;
    using Domain.Api;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class CreateUserBeerCommandHandler : IRequestHandler<CreateUserBeerCommand, BrewdudeApiResponse>
    {
        private readonly ILogger<CreateUserBeerCommandHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserBeerCommandHandler(BrewdudeDbContext context, ILogger<CreateUserBeerCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse> Handle(CreateUserBeerCommand request, CancellationToken cancellationToken)
        {
            // Validate the beer exists from the request
            var existingBeer = await _context.Beers.FindAsync(request.BeerId);

            if (existingBeer == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"Beer with ID [{request.BeerId}] was not added to user ID [{request.UserId}], beer does not exist");
            }

            var existingUserBeers = await _context.UserBeers
                .Where(ub => ub.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            // Validate the request beer to add does not already exist for the user
            if (existingUserBeers.Exists(ub => ub.BeerId == request.BeerId))
            {
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"User beer [{request.UserId}] already contains beer [{request.BeerId}]");
            }

            // Map to user beer entity and add to context
            var userBeer = _mapper.Map<UserBeer>(request);
            await _context.AddAsync(userBeer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User [{request.UserId}] has added beer [{request.BeerId}] successfully");

            return new BrewdudeApiResponse(
                (int)HttpStatusCode.Created,
                BrewdudeResponseMessage.Created.GetDescription());
        }
    }
}