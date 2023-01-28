using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.User;
using KeyVault.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyVault.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController: ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserForHome>>> Filter([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Contains(","))
            {
                return BadRequest("Invalid data provided!");
            }

            var users = await _userServices.FilterByUserName(userName);

            return Ok(users);
        }

        [HttpGet("login")]
        public async Task<ActionResult<UserForHome>> LoginUser([FromQuery] string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Empty data provided");
            }

            if (email.Contains("'") || password.Contains("'"))
            {
                return BadRequest("Invalid data provided");
            }

            var user = await _userServices.LoginUser(email, password);
            
            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserForHome>> CreateUser([FromBody] UserForCreation user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Empty data provided");
            }

            if (string.IsNullOrEmpty(user.UserName) || user.Email.Contains("'") || user.Password.Contains("'"))
            {
                return BadRequest("Invalid data provided");
            }

            var checkIfUserExists = await _userServices.LoginUser(user.Email, user.Password);

            if (checkIfUserExists != null)
            {
                return BadRequest("User already exists with these credentials");
            }

            var result = await _userServices.CreateUser(user);
            return Ok(result);
        }
    }
}