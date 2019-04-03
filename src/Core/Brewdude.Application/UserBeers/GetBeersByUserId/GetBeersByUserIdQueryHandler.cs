using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Common.Extensions;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.UserBeers.GetBeersByUserId
{
    public class GetBeersByUserIdQueryHandler : IRequestHandler<GetBeersByUserIdQuery, BrewdudeApiResponse<UserBeersViewModel>>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeersByUserIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<UserBeersViewModel>> Handle(GetBeersByUserIdQuery request, CancellationToken cancellationToken)
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

            var userBeersViewModel = new UserBeersViewModel
            {
                UserBeers = _mapper.Map<IEnumerable<UserBeerDto>>(userBeers),
                UserId = request.UserId,
                CanEdit = true
            };
            
            return new BrewdudeApiResponse<UserBeersViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                userBeersViewModel,
                userBeersViewModel.Count);
        }
    }
}