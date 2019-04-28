namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.Brewery.Commands.CreateBrewery;
    using Application.Brewery.Commands.DeleteBrewery;
    using Application.Brewery.Commands.UpdateBrewery;
    using Application.Brewery.Queries.GetAllBreweries;
    using Application.Brewery.Queries.GetBreweriesByState;
    using Application.Brewery.Queries.GetBreweryById;
    using Application.Brewery.Queries.GetBreweryByName;
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

        [HttpGet("search/name")]
        public async Task<ActionResult> GetBreweryByName(string breweryName)
        {
            _logger.LogInformation($"Searching for brewery [{breweryName}] for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetBreweryByNameQuery(breweryName)));
        }

        [HttpGet("search/state")]
        public async Task<ActionResult> GetBreweriesByState(string stateCode)
        {
            _logger.LogInformation($"Search breweries with state code [{stateCode}] for use [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetBreweriesByStateQuery(stateCode)));
        }

        [HttpPost]
        [Authorize("WriteBreweryPolicy")]
        public async Task<ActionResult> CreateBrewery([FromBody] CreateBreweryCommand createBreweryCommand)
        {
            _logger.LogInformation($"Creating brewery for request [{createBreweryCommand.Name}]");
            return Ok(await Mediator.Send(createBreweryCommand));
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