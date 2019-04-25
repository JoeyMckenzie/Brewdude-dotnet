using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Common;
using Brewdude.Common.Utilities;
using Brewdude.Domain.Api;
using Brewdude.Domain.Entities;
using Brewdude.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace Brewdude.Application.Tests.Beers.Commands
{
    [Collection("CommandCollection")]
    public class CreateBeerCommandHandlerTest
    {
        private readonly IDateTime _dateTime;
        private readonly BrewdudeDbContext _context;
        private readonly ILogger<CreateBeerCommandHandler> _logger;

        public CreateBeerCommandHandlerTest(TestFixture fixture)
        {
            _context = fixture.Context;
            _dateTime = fixture.MachineDateTime;
            _logger = NullLogger<CreateBeerCommandHandler>.Instance;
        }

        [Fact]
        public async Task CreateBeerCommand_GivenValidCreateCommand_ReturnsCreatedResponse()
        {
            // Arrange
            var createBeerCommand = new CreateBeerCommand
            {
                Name = "Dog Years IPA",
                Abv = 7.0,
                Ibu = 110,
                BeerStyle = BeerStyle.Ipa,
                Description = "A great IPA",
                BreweryId = 1
            };
            
            // Act
            var handler = new CreateBeerCommandHandler(_context, _dateTime, _logger);
            var result = await handler.Handle(createBeerCommand, CancellationToken.None);
            
            // Assert
            result.ShouldBeOfType<BrewdudeApiResponse>();
            result.StatusCode.ShouldBe(201);
            result.Success.ShouldBeTrue();
            _context.Beers.Single(b => b.Name == "Dog Years IPA").ShouldNotBeNull();
        }
    }
}