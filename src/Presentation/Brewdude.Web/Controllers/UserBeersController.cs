using System;
using System.Threading.Tasks;
using Brewdude.Application.UserBeers.GetBeersByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
    public class UserBeersController : BrewdudeControllerBase
    {
        private readonly ILogger<UserBeersController> _logger;

        public UserBeersController(ILogger<UserBeersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserBeersViewModel>> GetBeersByUserId(int id)
        {
            _logger.LogInformation($"Retrieving beers for user [{id}]");

            try
            {
                return Ok(await Mediator.Send(new GetBeersByUserIdQuery(id)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}