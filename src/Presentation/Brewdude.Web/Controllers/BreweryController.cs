namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.Brewery.Commands.CreateBrewery;
    using Application.Brewery.Commands.DeleteBrewery;
    using Application.Brewery.Commands.UpdateBrewery;
    using Application.Brewery.Queries.GetAllBreweries;
    using Application.Brewery.Queries.GetBreweryById;
    using Common.Extensions;
    using Domain.Api;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Middleware.Filters;

    [Authorize(Policy = "BrewdudeUserPolicy")]
    [ModelStateValidationActionFilter]
    public class BreweryController : BrewdudeControllerBase
    {
        private readonly ILogger<BreweryController> _logger;

        public BreweryController(ILogger<BreweryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBreweries()
        {
            _logger.LogInformation("Retrieving all breweries");
            return Ok(await Mediator.Send(new GetAllBreweriesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            _logger.LogInformation($"Retrieving brewery with id [{id}]");
            return Ok(await Mediator.Send(new GetBreweryByIdQuery(id)));
        }

        [HttpPost]
        [Authorize("WriteBreweryPolicy")]
        public async Task<ActionResult> CreateBrewery([FromBody] CreateBreweryCommand createBreweryCommand)
        {
            _logger.LogInformation($"Creating brewery for request [{createBreweryCommand.Name}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(createBreweryCommand));
        }

        [HttpPut("{id}")]
        [Authorize("WriteBreweryPolicy")]
        public async Task<ActionResult> UpdateBrewery(int id, [FromBody] UpdateBreweryCommand updateBreweryCommand)
        {
            _logger.LogInformation($"Updating brewery [{id}]");
            updateBreweryCommand.BreweryId = id;
            return Ok(await Mediator.Send(updateBreweryCommand));
        }

        [HttpDelete("{id}")]
        [Authorize("WriteBreweryPolicy")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            _logger.LogInformation($"Deleting brewery [{id}]");
            return Ok(await Mediator.Send(new DeleteBreweryCommand(id)));
        }
    }
}