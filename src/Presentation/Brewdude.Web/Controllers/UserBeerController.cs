namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.UserBeers.Commands.CreateUserBeer;
    using Application.UserBeers.Commands.DeleteUserBeer;
    using Application.UserBeers.Queries.GetBeersByUserId;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize(Policy = "BrewdudeUserPolicy")]
    public class UserBeerController : BrewdudeControllerBase
    {
        private readonly ILogger<UserBeerController> _logger;

        public UserBeerController(ILogger<UserBeerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetBeersByUserId(string userId)
        {
            _logger.LogInformation($"Retrieving beers for user [{userId}]");
            return Ok(await Mediator.Send(new GetBeersByUserIdQuery(userId)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserBeer([FromBody] CreateUserBeerCommand command)
        {
            _logger.LogInformation($"Creating beer [{command.BeerId}] for user [{command.UserId}]");
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserBeer([FromBody] DeleteUserBeerCommand command)
        {
            _logger.LogInformation($"Deleting beer [{command.UserId}] for user [{command.UserId}]");
            return Ok(await Mediator.Send(command));
        }
    }
}