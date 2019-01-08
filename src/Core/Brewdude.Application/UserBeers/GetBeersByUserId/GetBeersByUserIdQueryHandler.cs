using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class GetBeersByUserIdQueryHandler : IRequestHandler<GetBeersByUserIdQuery, UserBeersViewModel>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeersByUserIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserBeersViewModel> Handle(GetBeersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userBeers = await (
                from b in _context.Beers
                join ub in _context.UserBeers
                    on b.BeerId equals ub.BeerId
                join u in _context.Users
                    on ub.UserId equals u.UserId
                where u.UserId == request.UserId
                select b
            ).ToListAsync(cancellationToken);

            if (userBeers == null || userBeers.Count == 0)
                throw new BeerNotFoundException($"Could not find beers for user [{request.UserId}]");

            var userBeersViewModel = new UserBeersViewModel
            {
                UserBeers = _mapper.Map<IEnumerable<UserBeerDto>>(userBeers),
                UserId = request.UserId,
                CanEdit = true
            };
            
            return userBeersViewModel;
        }
    }
}