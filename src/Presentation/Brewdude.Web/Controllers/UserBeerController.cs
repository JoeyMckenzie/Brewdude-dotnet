namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.UserBeers.Commands.CreateUserBeer;
    using Application.UserBeers.Commands.DeleteUserBeer;
    using Application.UserBeers.Queries.GetBeersByUserId;
    using Common.Extensions;
    using Domain.Api;
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBeersByUserId(string id)
        {
            _logger.LogInformation($"Retrieving beers for user [{id}]");
            return Ok(await Mediator.Send(new GetBeersByUserIdQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserBeer(CreateUserBeerCommand command)
        {
            _logger.LogInformation($"Creating beer [{command.BeerId}] for user [{command.UserId}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserBeer(int id)
        {
            _logger.LogInformation($"Deleting beer [{id}] for user [{User.Identity.Name}]");
            return Ok(await Mediator.Send(new DeleteUserBeerCommand(id, User.Identity.Name)));
        }
    }
}