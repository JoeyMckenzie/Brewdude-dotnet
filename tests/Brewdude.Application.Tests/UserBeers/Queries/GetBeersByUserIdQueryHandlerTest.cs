using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Application.UserBeers.Queries.GetBeersByUserId;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Brewdude.Persistence;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Brewdude.Application.Tests.UserBeers.Queries
{
    [Collection("QueryCollection")]
    public class GetBeersByUserIdQueryHandlerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly BrewdudeDbContext _context;
        private readonly TestFixture _testFixture;
        private readonly IMapper _mapper;
        private readonly string _userId;

        public GetBeersByUserIdQueryHandlerTest(TestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testFixture = fixture;
            _context = fixture.Context;
            _mapper = fixture.Mapper;
            _userId = fixture.UserId;
        }

        [Fact]
        public async Task GetBeersByUserId_GivenValidUserWithBeers_ReturnsProperResponse()
        {
            // Arrange
            var handler = new GetBeersByUserIdQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(new GetBeersByUserIdQuery(_userId), CancellationToken.None);
            var userBeerId1 = result.Result.Results.FirstOrDefault(ub => ub.BeerId == 1);
            var userBeerId3 = result.Result.Results.FirstOrDefault(ub => ub.BeerId == 3);

            // Assert
            result.ShouldBeOfType<BrewdudeApiResponse<UserBeerListViewModel>>();
            userBeerId1.ShouldNotBeNull();
            userBeerId3.ShouldNotBeNull();
            userBeerId1.BeerId.ShouldBe(1);
            userBeerId3.BeerId.ShouldBe(3);
        }
        
        [Fact]
        public async Task GetBeersByUserId_GivenInvalidUserId_ThrowsNotFoundException()
        {
            // Arrange
            var handler = new GetBeersByUserIdQueryHandler(_context, _mapper);

            // Act/Assert
            await Should.ThrowAsync<BrewdudeApiException>(async () => 
                await handler.Handle(new GetBeersByUserIdQuery(Guid.NewGuid().ToString()), CancellationToken.None));
        }
    }
}