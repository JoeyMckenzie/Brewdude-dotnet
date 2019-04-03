using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Queries.GetBreweryById
{
    public class GetBreweryByIdQueryHandler : IRequestHandler<GetBreweryByIdQuery, BrewdudeApiResponse<BreweryViewModel>>
    {
        private readonly ILogger<GetBreweryByIdQueryHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweryByIdQueryHandler(BrewdudeDbContext context, IMapper mapper, ILogger<GetBreweryByIdQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse<BreweryViewModel>> Handle(GetBreweryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving brewery with ID [{request.BreweryId}]");
            
            var brewery = await _context.Breweries
                .Include(b => b.Beers)
                .SingleOrDefaultAsync(b => b.BreweryId == request.BreweryId, cancellationToken);
            
            if (brewery == null)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"Brewery [{request.BreweryId}] not found");
            
            return new BrewdudeApiResponse<BreweryViewModel>(
                (int)HttpStatusCode.OK, 
                BrewdudeResponseMessage.Success.GetDescription(), 
                _mapper.Map<BreweryViewModel>(brewery), 
                1);
        }
    }
}