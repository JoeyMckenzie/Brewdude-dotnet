using System.Threading.Tasks;
using Brewdude.Application.UserBeers.Commands.CreateUserBeer;
using Brewdude.Application.UserBeers.Queries.GetBeersByUserId;
using Brewdude.Common.Extensions;
using Brewdude.Domain.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
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
    }
}