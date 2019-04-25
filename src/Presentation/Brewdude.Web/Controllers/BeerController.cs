namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.Beer.Commands.CreateBeer;
    using Application.Beer.Commands.DeleteBeer;
    using Application.Beer.Commands.UpdateBeer;
    using Application.Beer.Queries.GetAllBeers;
    using Application.Beer.Queries.GetBeerById;
    using Common.Extensions;
    using Domain.Api;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Middleware.Filters;

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
        [Authorize("WriteBeerPolicy")]
        public async Task<ActionResult> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            _logger.LogInformation($"Creating beer [{createBeerCommand.Name}] for user [{_userIdOnRequest}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(createBeerCommand));
        }

        [HttpPut("{id}")]
        [Authorize("WriteBeerPolicy")]
        public async Task<ActionResult> UpdateBeer(int id, [FromBody] UpdateBeerCommand updateBeerCommand)
        {
            _logger.LogInformation($"Updating beer [{id}]");
            updateBeerCommand.BeerId = id;
            return Ok(await Mediator.Send(updateBeerCommand));
        }

        [HttpDelete("{id}")]
        [Authorize("WriteBeerPolicy")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            _logger.LogInformation($"Deleting beer [{id}]");
            return Ok(await Mediator.Send(new DeleteBeerCommand(id)));
        }
    }
}