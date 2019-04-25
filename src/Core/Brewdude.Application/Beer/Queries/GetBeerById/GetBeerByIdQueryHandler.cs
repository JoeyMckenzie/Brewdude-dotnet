namespace Brewdude.Application.Beer.Queries.GetBeerById
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Extensions;
    using Domain.Api;
    using Domain.ViewModels;
    using MediatR;
    using Persistence;

    public class GetBeerByIdQueryHandler : IRequestHandler<GetBeerByIdQuery, BrewdudeApiResponse<BeerViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly BrewdudeDbContext _context;

        public GetBeerByIdQueryHandler(BrewdudeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BrewdudeApiResponse<BeerViewModel>> Handle(GetBeerByIdQuery request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FindAsync(request.BeerId);

            if (beer == null)
            {
                throw new BrewdudeApiException(HttpStatusCode.NotFound, BrewdudeResponseMessage.BeerNotFound, $"Beer [{request.BeerId}] not found");
            }

            return new BrewdudeApiResponse<BeerViewModel>(
                (int)HttpStatusCode.OK, 
                BrewdudeResponseMessage.Success.GetDescription(),
                _mapper.Map<BeerViewModel>(beer), 
                1);
        }
    }
}