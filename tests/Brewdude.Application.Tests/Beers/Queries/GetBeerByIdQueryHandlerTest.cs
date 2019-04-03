using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using Shouldly;
using Xunit;

namespace Brewdude.Application.Tests.Beers.Queries
{
    [Collection("QueryCollection")]
    public class GetBeerByIdQueryHandlerTest
    {
        private readonly BrewdudeDbContext _context;
        private readonly IMapper _mapper;

        public GetBeerByIdQueryHandlerTest(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetBeerById_GivenExistingBeerId_RetrievesCorrectBeer()
        {
            // Arrange
            var handler = new GetBeerByIdQueryHandler(_context, _mapper);
            var hexageniaBeerId = 1;
            
            // Act
            var result = await handler.Handle(new GetBeerByIdQuery(hexageniaBeerId), CancellationToken.None);

            // Assert
            result.ShouldBeOfType<BrewdudeApiResponse<BeerViewModel>>();
            result.Result.ShouldBeOfType<BeerViewModel>();
            result.Length.ShouldBe(1);
            result.Result.Name.ShouldBe("Hexagenia");
        }

        [Fact]
        public async Task GetBeerById_GivenNonExistingBeerId_ThrowsException()
        {
            // Arrange
            var handler = new GetBeerByIdQueryHandler(_context, _mapper);
            
            // Act/Assert
            await Should.ThrowAsync<BrewdudeApiException>(async () =>
                await handler.Handle(new GetBeerByIdQuery(24), CancellationToken.None));
        }
    }
}