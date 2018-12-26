using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.GetAllBeers.Queries;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    public class GetAllBeersQueryHandler : IRequestHandler<GetAllBeersQuery, BeerListViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBeersQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BeerListViewModel> Handle(GetAllBeersQuery request, CancellationToken cancellationToken)
        {
            var beers = await _context.Beers
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
    
            var model = new BeerListViewModel
            {
                Beers = _mapper.Map<IEnumerable<BeerDto>>(beers)
            };

            return model;
        }
    }
}