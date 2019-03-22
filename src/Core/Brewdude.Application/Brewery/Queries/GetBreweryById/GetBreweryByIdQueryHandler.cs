using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Brewery.Queries.GetBreweryById
{
    public class GetBreweryByIdQueryHandler : IRequestHandler<GetBreweryByIdQuery, BreweryViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweryByIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BreweryViewModel> Handle(GetBreweryByIdQuery request, CancellationToken cancellationToken)
        {
            var brewery = await _context.Breweries
                .Include(b => b.Beers)
                .SingleOrDefaultAsync(b => b.BreweryId == request.BreweryId, cancellationToken);
            
            if (brewery == null)
                throw new BreweryNotFound($"Brewery [{request.BreweryId}] not found");
            
            return _mapper.Map<BreweryViewModel>(brewery);
        }
    }
}