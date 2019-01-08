using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Exceptions;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Application.UserBeers.GetBeersByUserId;
using Brewdude.Persistence;
using Shouldly;
using Xunit;

namespace Brewdude.Application.Tests.UserBeers.Queries
{
    [Collection("QueryCollection")]
    public class GetBeersByUserIdQueryHandlerTest
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeersByUserIdQueryHandlerTest(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetBeersByUserId_GivenValidUserWithBeers_ReturnsProperViewModel()
        {
            var handler = new GetBeersByUserIdQueryHandler(_context, _mapper);

            var result = await handler.Handle(new GetBeersByUserIdQuery(1), CancellationToken.None);

            result.ShouldBeOfType<UserBeersViewModel>();
            result.UserBeers.Count().ShouldBe(2);
        }
        
        [Fact]
        public async Task GetBeersByUserId_GivenInvalidUserId_ThrowsNotFoundException()
        {
            var handler = new GetBeersByUserIdQueryHandler(_context, _mapper);

            await Should.ThrowAsync<BeerNotFoundException>(async () =>
                await handler.Handle(new GetBeersByUserIdQuery(2), CancellationToken.None));
        }
    }
}