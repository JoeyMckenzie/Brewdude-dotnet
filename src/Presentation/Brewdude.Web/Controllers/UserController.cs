using System;
using System.Net;
using System.Threading.Tasks;
using Brewdude.Application.Exceptions;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Brewdude.Domain.ViewModels;
using Brewdude.Middleware.Models;
using Brewdude.Middleware.Wrappers;
using Brewdude.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    public class UserController : BrewdudeControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] CreateUserCommand createUserCommand)
        {
            UserViewModel user;
            _logger.LogInformation($"Sending request to create user {createUserCommand.Username} with email {createUserCommand.Email}");

            try
            {
                user = await Mediator.Send(createUserCommand);
            }
            catch (Exception e)
            {
                return HandleUserErrorMessage(e);
            }

            if (user == null)
                return BadRequest(new BrewdudeErrorViewModel("Unexpected system error attempting to create user, please try again"));

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ApiResponse> Login([FromBody] GetUserByUsernameCommand request)
        {
            _logger.LogInformation($"Processing login attempt for user {request.Username}");
            ApiResponse apiResponse;

            try
            {
                var user = await Mediator.Send(new GetUserByUsernameCommand(request.Username, request.Password));

                if (user == null)
                {
                    throw new ApiException("User was not found", (int)HttpStatusCode.NotFound);
                }
                    
                apiResponse = new ApiResponse((int)HttpStatusCode.OK, $"User {user.UserName} logged in successfully", user);

            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

            return apiResponse;
        }
        
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetUserById(int id)
        {
            UserViewModel user;

            try
            {
                user = await Mediator.Send(new GetUserByIdCommand(id));
            }
            catch (Exception e)
            {
                return HandleUserErrorMessage(e);
            }

            if (user == null)
                return BadRequest("Error retrieving user");

            return Ok(user);
        }

        private ActionResult<ApiResponse> HandleUserErrorMessage(Exception e)
        {
            var errorResponse = new ApiResponse((int)HttpStatusCode.BadRequest, e.Message);
                
            if (e is UserNotFoundException)
                return NotFound(errorResponse);

            return BadRequest(errorResponse);
        }
    }
}