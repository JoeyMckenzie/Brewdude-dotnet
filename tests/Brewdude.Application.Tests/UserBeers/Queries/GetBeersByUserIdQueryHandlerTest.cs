using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Application.UserBeers.GetBeersByUserId;
using Brewdude.Domain;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
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
        private readonly string _userId;

        public GetBeersByUserIdQueryHandlerTest(TestFixture fixture)
        {
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

            // Assert
            result.ShouldBeOfType<BrewdudeApiResponse<UserBeersViewModel>>();
            result.Result.Count.ShouldBe(2);
            result.Result.UserBeers.Single(ub => ub.BeerId == 1).ShouldNotBeNull();
            result.Result.UserBeers.Single(ub => ub.BeerId == 3).ShouldNotBeNull();
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