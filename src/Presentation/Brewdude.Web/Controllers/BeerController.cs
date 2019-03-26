using System.Threading.Tasks;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Common.Extensions;
using Brewdude.Domain.Api;
using Brewdude.Middleware.Models;
using Brewdude.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
    [ModelStateValidationActionFilter]
    public class BeerController : BrewdudeControllerBase
    {
        private readonly ILogger<BeerController> _logger;
        private readonly string _userIdOnRequest;

        public BeerController(ILogger<BeerController> logger)
        {
            _logger = logger;
            _userIdOnRequest = string.Empty;
            if (User?.Identity != null)
            {
                _userIdOnRequest = User.Identity.Name;
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBeers()
        {
            _logger.LogInformation($"Retrieving all beers for user [{_userIdOnRequest}]");
            return Ok(await Mediator.Send(new GetAllBeersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            _logger.LogInformation($"Retrieving beer [{id}] for user [{_userIdOnRequest}]");
            return Ok(await Mediator.Send(new GetBeerByIdQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            _logger.LogInformation($"Creating beer [{createBeerCommand.Name}] for user [{_userIdOnRequest}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(createBeerCommand));
            // return Ok(await Mediator.Send(createBeerCommand));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBeer(int id, [FromBody] UpdateBeerCommand updateBeerCommand)
        {
            _logger.LogInformation($"Updating beer [{id}]");
            updateBeerCommand.BeerId = id;
            return Ok(await Mediator.Send(updateBeerCommand));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            _logger.LogInformation($"Deleting beer [{id}]");
            return Ok(await Mediator.Send(new DeleteBeerCommand(id)));
        }
    }
}