using System;
using KeyVault.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KeyVault.Controllers
{
    [Route("api/crypto")]
    [ApiController]
    public class CryptoController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CryptoController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        
        [HttpGet]
        public ActionResult EncryptData()
        {
            var tool = new CryptoTool();
            
            var key = Guid.Parse(_configuration["AppSettings:Key"]);
            var iv = Guid.Parse(_configuration["AppSettings:Iv"]);
            
            var secret = tool.Encrypt("Hey there", key, iv);
            var decrypt = tool.Decrypt(secret, key, iv);

            var jwt = new TokenTools().GenerateJwt(
                _configuration["JWT:Secret"],
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]
            );

            return Ok(new { hidden = secret, unhiden = decrypt, jwt = jwt});
        }
    }
}