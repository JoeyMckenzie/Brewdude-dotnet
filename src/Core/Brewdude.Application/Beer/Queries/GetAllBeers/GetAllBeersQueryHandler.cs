namespace Brewdude.Application.Beer.Queries.GetAllBeers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Extensions;
    using Domain.Api;
    using Domain.Dtos;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

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
                Results = _mapper.Map<IEnumerable<BeerDto>>(beers)
            };

            return new BrewdudeApiResponse<BeerListViewModel>(
                (int)HttpStatusCode.OK,
                BrewdudeResponseMessage.Success.GetDescription(),
                viewModel,
                viewModel.Count);
        }
    }
}