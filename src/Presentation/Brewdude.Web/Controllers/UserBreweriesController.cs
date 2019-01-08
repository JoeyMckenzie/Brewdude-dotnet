using System;
using System.Threading.Tasks;
using Brewdude.Application.UserBreweries.GetBreweriesByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
    public class UserBreweriesController : BrewdudeControllerBase
    {
        private readonly ILogger<UserBreweriesController> _logger;

        public UserBreweriesController(ILogger<UserBreweriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserBreweriesViewModel>> GetBreweriesByUserId(int id)
        {
            _logger.LogInformation($"Retrieving breweries for user [{id}]");

            try
            {
                return Ok(await Mediator.Send(new GetBreweriesByUserIdQuery(id)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}