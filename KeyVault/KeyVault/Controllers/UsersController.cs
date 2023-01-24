using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Services.Users;
using KeyVault.Tools;
using Microsoft.AspNetCore.Mvc;

namespace KeyVault.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userServices.GetUsers();
            return Ok(users);
        }

        [HttpGet("crypto")]
        public ActionResult EncryptData()
        {
            var tool = new CryptoTool();
            var key = Guid.NewGuid();
            var iv = Guid.NewGuid();
            var secret = tool.Encrypt("Hey there", key, iv);
            var decrypt = tool.Decrypt(secret, key, iv); 

            return Ok(new { hidden = secret, unhiden = decrypt});
        }
    }
}