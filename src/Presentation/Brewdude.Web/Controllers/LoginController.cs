using System;
using System.Threading.Tasks;
using Brewdude.Jwt.Models;
using Brewdude.Jwt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;

        public LoginController(ILogger<LoginController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Login([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("User login attempt is invalid");

            UserViewModel userViewModel;

            try
            {
                userViewModel = await _userService.LoginUserByUsername(userDto.Username, userDto.Password);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (userViewModel == null)
                return BadRequest($"There was an error attempting to login user [{userDto.Username}]");

            return userViewModel;
        }
    }
}