using System.Threading.Tasks;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Brewdude.Domain.Api;
using Brewdude.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    public class UserController : BrewdudeControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<BrewdudeApiResponse<UserViewModel>> Register([FromBody] CreateUserCommand createUserCommand)
        {
            _logger.LogInformation($"Sending request to create user {createUserCommand.Username} with email {createUserCommand.Email}");
            return await Mediator.Send(createUserCommand);
        }

        [HttpPost]
        [Route("login")]
        public async Task<BrewdudeApiResponse<UserViewModel>> Login([FromBody] GetUserByUsernameCommand request)
        {
            _logger.LogInformation($"Processing login attempt for user {request.Username}");
            return await Mediator.Send(new GetUserByUsernameCommand(request.Username, request.Password));
        }
        
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetUserById(int id)
        {
            _logger.LogInformation($"Retrieving user [{id}]");
            return Ok(await Mediator.Send(new GetUserByIdCommand(id)));
        }
    }
}