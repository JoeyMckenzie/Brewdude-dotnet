using System;
using System.Threading.Tasks;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Beer.GetAllBeers.Queries;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Brewdude.Web.Controllers
{
        [Authorize(Policy = "BrewdudeUserPolicy")]
    public class BeerController : BrewdudeControllerBase
    {
        private readonly ILogger<BeerController> _logger;

        public BeerController(ILogger<BeerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BeerListViewModel>> GetAll()
        {
            _logger.LogInformation("Test");
            return Ok(await Mediator.Send(new GetAllBeersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerViewModel>> GetById(int id)
        {
            try
            {
                var beer = await Mediator.Send(new GetBeerByIdQuery(id));
                return Ok(beer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            Log.Information("CreateBeer test");
            return Ok(await Mediator.Send(createBeerCommand));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBeer(int id, [FromBody] UpdateBeerCommand updateBeerCommand)
        {
            try
            {
                updateBeerCommand.BeerId = id;
                var result = await Mediator.Send(updateBeerCommand);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteBeerCommand(id));
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}