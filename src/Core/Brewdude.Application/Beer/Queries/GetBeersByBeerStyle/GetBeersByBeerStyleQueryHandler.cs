namespace Brewdude.Application.Beer.Queries.GetBeersByBeerStyle
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
    using Domain.Entities;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class GetBeersByBeerStyleQueryHandler : IRequestHandler<GetBeersByBeerStyleQuery, BrewdudeApiResponse<BeerListViewModel>>
    {
        private readonly ILogger<GetBeersByBeerStyleQueryHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeersByBeerStyleQueryHandler(ILogger<GetBeersByBeerStyleQueryHandler> logger, BrewdudeDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<BeerListViewModel>> Handle(GetBeersByBeerStyleQuery request, CancellationToken cancellationToken)
        {
            // Validate the beer style can be converted into an enumerated type
            var validateBeerStyleFromRequest = Enum.TryParse(request.BeerStyle, true, out BeerStyle validatedBeerStyle);

            if (!validateBeerStyleFromRequest)
            {
                _logger.LogError($"Invalid beer style request on search: [{request.BeerStyle}]");
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"Beer style [{request.BeerStyle}] is not valid");
            }

            // Search for all beers with the corresponding beer style
            var searchResults = await _context.Beers
                .Where(b => b.BeerStyle == validatedBeerStyle)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            var beerStylesViewModel = new BeerListViewModel
            {
                Results = _mapper.Map<IEnumerable<BeerDto>>(searchResults)
            };

            return new BrewdudeApiResponse<BeerListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                beerStylesViewModel,
                beerStylesViewModel.Count);
        }
    }
}