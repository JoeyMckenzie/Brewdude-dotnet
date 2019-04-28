namespace Brewdude.Application.Beer.Queries.GetBeerByName
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Extensions;
    using Domain.Api;
    using Domain.Dtos;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class GetBeerByNameQueryHandler : IRequestHandler<GetBeerByNameQuery, BrewdudeApiResponse<BeerListViewModel>>
    {
        private readonly ILogger<GetBeerByNameQueryHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeerByNameQueryHandler(BrewdudeDbContext context, IMapper mapper, ILogger<GetBeerByNameQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse<BeerListViewModel>> Handle(GetBeerByNameQuery request, CancellationToken cancellationToken)
        {
            // Search the database for all beers with names containing the request beer name
            var searchResults = await _context.Beers
                .Where(b => b.Name.Contains(request.BeerName, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            if (searchResults == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"No beers found with name [{request.BeerName}]");
            }

            var beerListSearchResults = new BeerListViewModel
            {
                Results = _mapper.Map<IEnumerable<BeerDto>>(searchResults)
            };
            _logger.LogInformation($"Returning [{beerListSearchResults.Count}] results for search request [{request.BeerName}]");

            return new BrewdudeApiResponse<BeerListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                beerListSearchResults,
                beerListSearchResults.Count);
        }
    }
}