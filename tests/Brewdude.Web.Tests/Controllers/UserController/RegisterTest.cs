using System.Net.Http;
using System.Threading.Tasks;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Domain.Entities;
using Brewdude.Web.Tests.Common;
using Xunit;

namespace Brewdude.Web.Tests.Controllers.UserController
{
    public class RegisterTest : IClassFixture<BrewdudeWebApplicationFactory<Startup>>
    {
        private const string Server = "https://localhost:5001";
        private readonly HttpClient _httpClient;

        public RegisterTest(BrewdudeWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_GivenValidCreateUserCommand_ReturnsProperResponse()
        {
            var createUserCommand = new CreateUserCommand
            {
                Username = "Test",
                Password = "#MyTestPassword!",
                FirstName = "Wayne",
                LastName = "Campbell",
                Email = "wayne.campbell@waynesworld.com",
                Role = Role.User
            };
            
            var content = Utilities.GetRequestContent(createUserCommand);
            var response = await _httpClient.PostAsync($"{Server}/user/register", content);
            response.EnsureSuccessStatusCode();
            var responseString = await Utilities.GetResponseContent<string>(response);
        }
    }
}