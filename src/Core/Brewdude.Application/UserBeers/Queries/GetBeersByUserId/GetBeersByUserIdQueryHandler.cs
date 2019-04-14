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

namespace Brewdude.Application.UserBeers.Queries.GetBeersByUserId
{
    public class GetBeersByUserIdQueryHandler : IRequestHandler<GetBeersByUserIdQuery, BrewdudeApiResponse<UserBeerListViewModel>>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeersByUserIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<UserBeerListViewModel>> Handle(GetBeersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userBeers = await (
                from b in _context.Beers
                join ub in _context.UserBeers
                    on b.BeerId equals ub.BeerId
                where ub.UserId == request.UserId 
                select b
            ).ToListAsync(cancellationToken);

            if (userBeers == null || userBeers.Count == 0)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"Could not find beers for user [{request.UserId}]");

            var userBeerListViewModel = new UserBeerListViewModel
            {
                Results = _mapper.Map<IEnumerable<BeerViewModel>>(userBeers),
                UserId = request.UserId,
            };
            
            return new BrewdudeApiResponse<UserBeerListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                userBeerListViewModel,
                userBeerListViewModel.Count);
        }
    }
}