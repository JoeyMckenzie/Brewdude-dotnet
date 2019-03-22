using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Brewery.Queries.GetAllBreweries
{
    public class GetAllBreweriesQueryHandler : IRequestHandler<GetAllBreweriesQuery, BreweryViewModelList>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBreweriesQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BreweryViewModelList> Handle(GetAllBreweriesQuery request, CancellationToken cancellationToken)
        {
            var breweries = await _context.Breweries
                .Include(b => b.Beers)
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            var model = new BreweryViewModelList
            {
                Breweries = _mapper.Map<IEnumerable<BreweryViewModel>>(breweries)
            };

            return model;
        }
    }
}