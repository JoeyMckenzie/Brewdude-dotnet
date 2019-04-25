namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.User.Commands.CreateUser;
    using Application.User.Commands.DeleteUser;
    using Application.User.Commands.UpdateUser;
    using Application.User.Queries.GetUserById;
    using Application.User.Queries.GetUserByUsername;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class UserController : BrewdudeControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserCommand createUserCommand)
        {
            _logger.LogInformation($"Sending request to create user {createUserCommand.Username} with email {createUserCommand.Email}");
            return Ok(await Mediator.Send(createUserCommand));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] GetUserByUsernameCommand request)
        {
            _logger.LogInformation($"Processing login attempt for user {request.Username}");
            return Ok(await Mediator.Send(new GetUserByUsernameCommand(request.Username, request.Password)));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetUserById(string id)
        {
            _logger.LogInformation($"Retrieving user [{id}]");
            return Ok(await Mediator.Send(new GetUserByIdCommand(id)));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand updateUserCommand)
        {
            _logger.LogInformation($"Updating user with ID {id}");
            updateUserCommand.UserId = id;
            return Ok(await Mediator.Send(updateUserCommand));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUserById(string id)
        {
            _logger.LogInformation($"Deleting user [{id}]");
            return Ok(await Mediator.Send(new DeleteUserCommand(id)));
        }
    }
}