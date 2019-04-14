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

namespace Brewdude.Application.UserBreweries.Queries.GetBreweriesByUserId
{
    public class GetBreweriesByUserIdQueryHandler : IRequestHandler<GetBreweriesByUserIdQuery, BrewdudeApiResponse<UserBreweryListViewModel>>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBreweriesByUserIdQueryHandler(IMapper mapper, BrewdudeDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BrewdudeApiResponse<UserBreweryListViewModel>> Handle(GetBreweriesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userBreweries = await (
                from b in _context.Breweries
                join ub in _context.UserBreweries
                    on b.BreweryId equals ub.BreweryId
                where ub.UserId == request.UserId
                select b
            )
                .Include(b => b.Beers)
                .Include(b => b.Address)
                .ToListAsync(cancellationToken);
            
            if (userBreweries == null || userBreweries.Count == 0)
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BreweryNotFound, $"No breweries found for user [{request.UserId}]");

            var userBreweriesViewModel = new UserBreweryListViewModel
            {
                Results = _mapper.Map<IEnumerable<BreweryViewModel>>(userBreweries),
                UserId = request.UserId,
            };

            return new BrewdudeApiResponse<UserBreweryListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                userBreweriesViewModel,
                userBreweriesViewModel.Count);
        }
    }
}