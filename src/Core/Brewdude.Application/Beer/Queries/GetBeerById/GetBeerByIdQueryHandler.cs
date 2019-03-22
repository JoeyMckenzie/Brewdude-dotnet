using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;

namespace Brewdude.Application.Beer.Queries.GetBeerById
{
    public class GetBeerByIdQueryHandler : IRequestHandler<GetBeerByIdQuery, BeerViewModel>
    {
        private readonly IMapper _mapper;
        private readonly BrewdudeDbContext _context;

        public GetBeerByIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BeerViewModel> Handle(GetBeerByIdQuery request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FindAsync(request.BeerId);
            
            if (beer == null)
                throw new BeerNotFoundException($"Beer [{request.BeerId}] not found");

            return _mapper.Map<BeerViewModel>(beer);
        }
    }
}