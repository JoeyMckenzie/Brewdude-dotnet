using System;
using System.Threading.Tasks;
using Brewdude.Application.Brewery.Commands.CreateBrewery;
using Brewdude.Application.Brewery.Queries.GetAllBreweries;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
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
        public async Task<ActionResult<BreweryViewModel>> GetById(int id)
        {
            _logger.LogInformation($"Retrieving brewery with id [{id}]");

            try
            {
                var brewery = await Mediator.Send(new GetBreweryByIdQuery(id));
                return Ok(brewery);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateBrewery([FromBody] CreateBreweryCommand createBreweryCommand)
        {
            _logger.LogInformation($"Creating brewery for request [{createBreweryCommand.Name}]");
            return Ok(await Mediator.Send(createBreweryCommand));
        }
    }
}