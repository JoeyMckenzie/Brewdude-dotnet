using System.Net.Http;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Brewdude.Web.Tests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace Brewdude.Web.Tests.Controllers.UserController
{
    public class LoginTest : IClassFixture<BrewdudeWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public LoginTest(BrewdudeWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public void Login_GivenValidUserCredentials_ReturnsJwtWithUserInformation()
        {
            var loginCommand = new GetUserByUsernameCommand("testUser", "testPassword");
            
            
            loginCommand.ShouldBeNull();
        }
    }
}