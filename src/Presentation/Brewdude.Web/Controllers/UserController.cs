using System;
using System.Threading.Tasks;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Commands.Models;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
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
        public async Task<ActionResult<UserViewModel>> Register([FromBody] CreateUserCommand createUserCommand)
        {
            UserViewModel user;

            try
            {
                user = await Mediator.Send(createUserCommand);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (user == null)
                return BadRequest("Error creating user");

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