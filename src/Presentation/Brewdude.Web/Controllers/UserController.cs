using System;
using System.Threading.Tasks;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Commands.Models;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public async Task<ActionResult<UserViewModel>> Register([FromBody] CreateUserCommand createUserCommand)
        {
            UserViewModel user;
            _logger.LogInformation($"Sending request to create user {createUserCommand.Username} with email {createUserCommand.Email}");

            try
            {
                user = await Mediator.Send(createUserCommand);
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject(e.Message));
            }

            if (user == null)
                return BadRequest(JsonConvert.SerializeObject("Unexpected system error attempting to create user, please try again"));

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(int id)
        {
            UserViewModel user;

            try
            {
                user = await Mediator.Send(new GetUserByIdCommand(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (user == null)
                return BadRequest("Error retrieving user");

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserViewModel>> Login([FromBody] GetUserByUsernameCommand request)
        {
            UserViewModel user;

            try
            {
                user = await Mediator.Send(new GetUserByUsernameCommand(request.Username, request.Password));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (user == null)
                return BadRequest("Error retrieving user");

            return Ok(user);
        }
    }
}