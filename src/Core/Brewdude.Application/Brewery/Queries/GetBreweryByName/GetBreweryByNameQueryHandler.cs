namespace Brewdude.Application.Brewery.Queries.GetBreweryByName
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
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetBreweryByNameQueryHandler : IRequestHandler<GetBreweryByNameQuery, BrewdudeApiResponse<BreweryListViewModel>>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweryByNameQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<BreweryListViewModel>> Handle(GetBreweryByNameQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _context.Breweries
                .Where(b => b.Name.Contains(request.BreweryName, StringComparison.CurrentCultureIgnoreCase))
                .Include(b => b.Address)
                .Include(b => b.Beers)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            if (searchResults == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"No breweries with name [{request.BreweryName}]");
            }

            var breweryListViewModel = new BreweryListViewModel
            {
                Results = _mapper.Map<IEnumerable<BreweryViewModel>>(searchResults)
            };

            return new BrewdudeApiResponse<BreweryListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                breweryListViewModel,
                breweryListViewModel.Count);
        }
    }
}