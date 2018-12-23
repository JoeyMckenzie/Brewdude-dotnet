using System;
using System.Threading.Tasks;
using Brewdude.Applicaio.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.GetAllBeers.Queries;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Brewdude.Web.Controllers
{
    public class BeerController : BrewdudeControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BeerListViewModel>> GetAll()
        {
            Log.Information("Test");
            return Ok(await Mediator.Send(new GetAllBeersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerViewModel>> GetById(int id)
        {
            Log.Information("GetById Test");
            return Ok(await Mediator.Send(new GetBeerByIdQuery(id)));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            Log.Information("CreateBeer test");
            return Ok(await Mediator.Send(createBeerCommand));
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