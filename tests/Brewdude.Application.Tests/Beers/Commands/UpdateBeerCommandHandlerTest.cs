using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Tests.Infrastructure;
using Brewdude.Domain;
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
    public class UpdateBeerCommandHandlerTest
    {
        private readonly BrewdudeDbContext _context;
        private readonly ILogger<UpdateBeerCommandHandler> _logger;

        public UpdateBeerCommandHandlerTest(TestFixture fixture)
        {
            _context = fixture.Context;
            _logger = NullLogger<UpdateBeerCommandHandler>.Instance;
        }

        [Fact]
        public async Task UpdateBeerCommand_GivenValidRequestForExistingBeer_UpdatesBeerAndReturnsOkResponse()
        {
            // Arrange
            var existingBeer = _context.Beers.FirstOrDefault(b => b.Name == "Hexagenia");
            var existingIbu = existingBeer.Ibu;
            var existingAbv = existingBeer.Abv;
            var existingDescription = existingBeer.Description;
            var existingBreweryId = existingBeer.BreweryId;
            var existingName = existingBeer.Name;
            var existingBeerStyle = existingBeer.BeerStyle;
            var updatedValues = new UpdateBeerCommand(existingBeer.BeerId)
            {
                Ibu = 99,
                Abv = 7.5,
                Description = "This is an updated description",
                BeerStyle = BeerStyle.Ipa,
                Name = "Hexagenia"
            };

            // Act
            var handler = new UpdateBeerCommandHandler(_context, _logger);
            var result = await handler.Handle(updatedValues, CancellationToken.None);

            // Assert
            var updatedBeer = await _context.Beers.FindAsync(existingBeer.BeerId);
            result.ShouldNotBeNull();
            result.ShouldBeOfType<BrewdudeApiResponse>();
            result.StatusCode.ShouldBe(200);
            
            // Validate values have been updated
            updatedBeer.Ibu.ShouldNotBeSameAs(existingIbu);
            updatedBeer.Abv.ShouldNotBeSameAs(existingAbv);
            updatedBeer.Description.ShouldNotBeSameAs(existingDescription);
            
            // Validate values that have not changed
            updatedBeer.BreweryId.ShouldBe(existingBreweryId);
            updatedBeer.Name.ShouldBe(existingName);
            updatedBeer.BeerStyle.ShouldBe(existingBeerStyle);
        }

        [Fact]
        public async Task UpdateBeerCommand_GivenNonExistingBeerId_ShouldThrowException()
        {
            // Arrange
            var handler = new UpdateBeerCommandHandler(_context, _logger);
            
            // Act/Assert
            await Should.ThrowAsync<BrewdudeApiException>(async () =>
                await handler.Handle(new UpdateBeerCommand(1337), CancellationToken.None));
        }
    }
}