using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.User;
using KeyVault.Services.Users;
using KeyVault.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KeyVault.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;

        public UsersController(IUserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userServices.GetUsers();
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
        
        [HttpGet("create")]
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

        [HttpGet("crypto")]
        public ActionResult EncryptData()
        {
            var tool = new CryptoTool();
            var key = Guid.Parse(_configuration.GetSection("AppSettings").GetSection("Key").Value);
            var iv = Guid.Parse(_configuration.GetSection("AppSettings").GetSection("Iv").Value);
            var secret = tool.Encrypt("Hey there", key, iv);
            var decrypt = tool.Decrypt(secret, key, iv); 

            return Ok(new { hidden = secret, unhiden = decrypt});
        }
    }
}