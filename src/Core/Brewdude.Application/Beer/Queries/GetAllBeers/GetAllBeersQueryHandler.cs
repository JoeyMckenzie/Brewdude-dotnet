using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Common.Extensions;
using Brewdude.Domain.Api;
using Brewdude.Domain.Dtos;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    public class GetAllBeersQueryHandler : IRequestHandler<GetAllBeersQuery, BrewdudeApiResponse<BeerListViewModel>>
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBeersQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<BeerListViewModel>> Handle(GetAllBeersQuery request, CancellationToken cancellationToken)
        {
            var beers = await _context.Beers
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);
    
            var viewModel = new BeerListViewModel
            {
                Beers = _mapper.Map<IEnumerable<BeerDto>>(beers)
            };

            return new BrewdudeApiResponse<BeerListViewModel>(
                (int)HttpStatusCode.OK, 
                BrewdudeResponseMessage.Success.GetDescription(), 
                viewModel,
                viewModel.Beers.Count());
        }
    }
}