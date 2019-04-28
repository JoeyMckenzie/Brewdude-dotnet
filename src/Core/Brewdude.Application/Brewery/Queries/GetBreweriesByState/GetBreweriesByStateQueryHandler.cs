namespace Brewdude.Application.Brewery.Queries.GetBreweriesByState
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Constants;
    using Common.Extensions;
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class GetBreweriesByStateQueryHandler : IRequestHandler<GetBreweriesByStateQuery, BrewdudeApiResponse<BreweryListViewModel>>
    {
        private readonly ILogger<GetBreweriesByStateQueryHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweriesByStateQueryHandler(ILogger<GetBreweriesByStateQueryHandler> logger, IMapper mapper, BrewdudeDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BrewdudeApiResponse<BreweryListViewModel>> Handle(GetBreweriesByStateQuery request, CancellationToken cancellationToken)
        {
            // Validate the state code on the request
            if (!BrewdudeConstants.ValidStateRegex.IsMatch(request.State.ToUpper(CultureInfo.CurrentCulture)))
            {
                throw new BrewdudeApiException(HttpStatusCode.BadRequest, BrewdudeResponseMessage.BadRequest, $"[{request.State}] is not a valid state code");
            }

            // Retrieve all breweries by state
            var searchResult = await _context.Breweries
                .Where(b => string.Equals(b.Address.State, request.State, StringComparison.CurrentCultureIgnoreCase))
                .Include(b => b.Beers)
                .Include(b => b.Address)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            if (searchResult == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"No breweries found for state code [{request.State}]");
            }

            var breweriesListViewModel = new BreweryListViewModel
            {
                Results = _mapper.Map<IEnumerable<BreweryViewModel>>(searchResult)
            };

            return new BrewdudeApiResponse<BreweryListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                breweriesListViewModel,
                breweriesListViewModel.Count);
        }
    }
}