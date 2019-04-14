using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class GetAllBreweriesQueryHandler : IRequestHandler<GetAllBreweriesQuery, BrewdudeApiResponse<BreweryListViewModel>>
    {
        private readonly ILogger<GetAllBreweriesQueryHandler> _logger;
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBreweriesQueryHandler(BrewdudeDbContext context, IMapper mapper, ILogger<GetAllBreweriesQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BrewdudeApiResponse<BreweryListViewModel>> Handle(GetAllBreweriesQuery request, CancellationToken cancellationToken)
        {
            var breweries = await _context.Breweries
                .Include(b => b.Beers)
                .Include(b => b.Address)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
            
            var viewModel = new BreweryListViewModel
            {
                Breweries = _mapper.Map<IEnumerable<BreweryViewModel>>(breweries),
            };

            return new BrewdudeApiResponse<BreweryListViewModel>(
                (int)HttpStatusCode.OK, 
                BrewdudeResponseMessage.Success.GetDescription(), 
                viewModel,
                viewModel.Count);
        }
    }
}