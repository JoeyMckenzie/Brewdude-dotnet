using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.UserBreweries.GetBreweriesByUserId
{
    public class GetBreweriesByUserIdQueryHandler : IRequestHandler<GetBreweriesByUserIdQuery, UserBreweriesViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweriesByUserIdQueryHandler(IMapper mapper, BrewdudeDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserBreweriesViewModel> Handle(GetBreweriesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userBreweries = await (
                from b in _context.Breweries
                join ub in _context.UserBreweries
                    on b.BreweryId equals ub.BreweryId
                select b
            ).Include(b => b.Beers)
                .ToListAsync(cancellationToken);
            
            if (userBreweries == null || userBreweries.Count == 0)
                throw new BreweryNotFound($"No breweries found for user [{request.UserId}]");

            var userBreweriesViewModel = new UserBreweriesViewModel
            {
                Breweries = _mapper.Map<IEnumerable<UserBreweryDto>>(userBreweries),
                UserId = request.UserId,
                CanEdit = true
            };

            return userBreweriesViewModel;
        }
    }
}