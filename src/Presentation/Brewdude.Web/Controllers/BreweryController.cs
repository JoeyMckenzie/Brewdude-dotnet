using System.Threading.Tasks;
using Brewdude.Application.Brewery.Commands.CreateBrewery;
using Brewdude.Application.Brewery.Commands.DeleteBrewery;
using Brewdude.Application.Brewery.Commands.UpdateBrewery;
using Brewdude.Application.Brewery.Queries.GetAllBreweries;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Brewdude.Common.Extensions;
using Brewdude.Domain.Api;
using Brewdude.Middleware.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
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
        public async Task<ActionResult> CreateBrewery([FromBody] CreateBreweryCommand createBreweryCommand)
        {
            _logger.LogInformation($"Creating brewery for request [{createBreweryCommand.Name}]");
            return Created(BrewdudeResponseMessage.Created.GetDescription(), await Mediator.Send(createBreweryCommand));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBrewery(int id, [FromBody] UpdateBreweryCommand updateBreweryCommand)
        {
            _logger.LogInformation($"Updating brewery [{id}]");
            updateBreweryCommand.BreweryId = id;
            return Ok(await Mediator.Send(updateBreweryCommand));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            _logger.LogInformation($"Deleting brewery [{id}]");
            return Ok(await Mediator.Send(new DeleteBreweryCommand(id)));
        }
    }
}