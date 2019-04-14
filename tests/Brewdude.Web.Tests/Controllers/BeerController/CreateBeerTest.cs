using System.Net.Http;
using System.Threading.Tasks;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Domain.Entities;
using Brewdude.Web.Tests.Common;
using Shouldly;
using Xunit;

namespace Brewdude.Web.Tests.Controllers.BeerController
{
    public class CreateBeerTest : IClassFixture<BrewdudeWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private const string Server = "https://localhost:5001";

        public CreateBeerTest(BrewdudeWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GivenCreateBeerCommand_WithValidStructure_ReturnsOkResponse()
        {
            var command = new CreateBeerCommand
            {
                Name = "Sand Dog IPA",
                Abv = 7.4,
                Ibu = 103,
                BeerStyle = BeerStyle.Ipa,
                Description = "A sandy IPA"
            };

            var content = Utilities.GetRequestContent(command);
            var response = await _httpClient.PostAsync($"{Server}/api/beer", content);
            response.EnsureSuccessStatusCode();
            var beerId = await Utilities.GetResponseContent<int>(response);

            beerId.ShouldNotBe(0);
        }
    }
}