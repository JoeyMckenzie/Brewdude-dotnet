using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Middleware.Models;
using Brewdude.Middleware.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    [Authorize(Policy = "BrewdudeUserPolicy")]
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
        public async Task<ActionResult<ApiResponse>> GetAllBeers()
        {
            _logger.LogInformation($"Retrieving all beers for user [{_userIdOnRequest}]");
            ApiResponse apiResponse;

            try
            {
                var beers = await Mediator.Send(new GetAllBeersQuery());
                
                if (beers == null)
                    throw new ApiException("No beers found", (int)HttpStatusCode.NotFound);

                apiResponse = new ApiResponse((int)HttpStatusCode.OK, "Beers retrieved successfully", beers)
                {
                    ResultLength = beers.Beers.Count()
                };
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            _logger.LogInformation($"Retrieving beer [{id}] for user [{_userIdOnRequest}]");
            ApiResponse apiResponse;
            
            try
            {
                var beer = await Mediator.Send(new GetBeerByIdQuery(id));
                
                if (beer == null)
                    throw new ApiException($"No beer with ID [{id}] found", (int)HttpStatusCode.NotFound);

                apiResponse = new ApiResponse((int)HttpStatusCode.OK, $"Beer [{beer.BeerId}] retrieved successfully", beer)
                {
                    ResultLength = 1
                };
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ApiResponse> CreateBeer([FromBody] CreateBeerCommand createBeerCommand)
        {
            _logger.LogInformation($"Creating beer [{createBeerCommand.Name}] for user [{_userIdOnRequest}]");
            ApiResponse apiResponse;

            try
            {
                var creationResult = await Mediator.Send(createBeerCommand);
                apiResponse = new ApiResponse((int)HttpStatusCode.OK, $"Beer with ID [{creationResult}] created successfully");
            }
            catch (Exception e)
            {
                throw new ApiException(e, (int)HttpStatusCode.BadRequest);
            }

            return apiResponse;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBeer(int id, [FromBody] UpdateBeerCommand updateBeerCommand)
        {
            _logger.LogInformation($"Updating beer [{id}]");
            
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
            _logger.LogInformation($"Deleting beer [{id}]");

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