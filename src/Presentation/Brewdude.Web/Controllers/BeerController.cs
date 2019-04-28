namespace Brewdude.Web.Controllers
{
    using System.Threading.Tasks;
    using Application.Beer.Commands.CreateBeer;
    using Application.Beer.Commands.DeleteBeer;
    using Application.Beer.Commands.UpdateBeer;
    using Application.Beer.Queries.GetAllBeers;
    using Application.Beer.Queries.GetBeerById;
    using Application.Beer.Queries.GetBeerByName;
    using Application.Beer.Queries.GetBeersByBeerStyle;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Middleware.Filters;

    [Authorize(Policy = "BrewdudeUserPolicy")]
    [ModelStateValidationActionFilter]
    public class BeerController : BrewdudeControllerBase
    {
        private readonly ILogger<BeerController> _logger;

        public BeerController(ILogger<BeerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBeers()
        {
            _logger.LogInformation($"Retrieving all beers for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetAllBeersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            _logger.LogInformation($"Retrieving beer [{id}] for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetBeerByIdQuery(id)));
        }

        [HttpGet("search/name")]
        public async Task<ActionResult> GetBeerByName(string beerName)
        {
            _logger.LogInformation($"Searching for beer [{beerName}] for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetBeerByNameQuery(beerName)));
        }

        [HttpGet("search/style")]
        public async Task<ActionResult> GetBeerByBeerStyle(string style)
        {
            _logger.LogInformation($"Searching for style [{style}] for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(new GetBeersByBeerStyleQuery(style)));
        }

        [HttpPost]
        [Authorize("WriteBeerPolicy")]
        public async Task<ActionResult> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            _logger.LogInformation($"Creating beer [{createBeerCommand.Name}] for user [{User.Identity?.Name}]");
            return Ok(await Mediator.Send(createBeerCommand));
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