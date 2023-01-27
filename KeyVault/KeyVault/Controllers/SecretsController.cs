using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.Secrets;
using KeyVault.Services.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyVault.Controllers
{
    [Route("api/secrets")]
    [ApiController]
    [Authorize]
    public class SecretsController: ControllerBase
    {
        private readonly ISecretsService _secretsService;

        public SecretsController(ISecretsService secretsService)
        {
            _secretsService = secretsService ?? throw new ArgumentNullException(nameof(secretsService));
        }

        [HttpGet]
        public async Task<ActionResult<Task<IEnumerable<Secret>>>> FilterSecrets([FromQuery] string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId) || ownerId.Contains(","))
            {
                return BadRequest("Invalid data provided");
            }

            var secrets = await _secretsService.GetSecrets(ownerId);
            return Ok(secrets);
        }

        [HttpPost]
        public async Task<ActionResult<Secret>> Create([FromBody] SecretForCreation secret)
        {
            if (
                string.IsNullOrEmpty(secret.Content) ||
                string.IsNullOrEmpty(secret.Title) ||
                string.IsNullOrEmpty(secret.OwnerId)
            )
            {
                return BadRequest("Invalid data provided");
            }

            var newSecret = await _secretsService.Create(secret); 
            return Ok(newSecret);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string secretId)
        {
            if (string.IsNullOrEmpty(secretId) || secretId.Contains("'"))
            {
                return BadRequest("Invalid data provided");
            }

            await _secretsService.Delete(secretId);
            return Ok("Secret deleted successfully!");
        }
    }
}