using System.Threading.Tasks;
using Brewdude.Application.UserBreweries.Commands;    
using Brewdude.Application.UserBreweries.Queries.GetBreweriesByUserId;
using Brewdude.Common.Extensions;
using Brewdude.Domain.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
    public class UserBreweryController : BrewdudeControllerBase
    {
        private readonly ILogger<UserBreweryController> _logger;

        public UserBreweryController(ILogger<UserBreweryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBreweriesByUserId(string id)
        {
            _logger.LogInformation($"Retrieving breweries for user [{id}]");
            return Ok(await Mediator.Send(new GetBreweriesByUserIdQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserBrewery(CreateUserBreweryCommand command)
        {
            _logger.LogInformation($"Creating brewery [{command.BreweryId}] for user [{command.UserId}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(command));
        }
    }
}