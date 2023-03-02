using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.Secrets;
using KeyVault.Services.Groups;
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
        private readonly IGroupsService _groupsService;
        public SecretsController(ISecretsService secretsService, IGroupsService groupsService)
        {
            _secretsService = secretsService ?? throw new ArgumentNullException(nameof(secretsService));
            _groupsService = groupsService ?? throw new ArgumentNullException(nameof(groupsService));
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
        public async Task<ActionResult<SecretForHome>> Create([FromBody] SecretForCreation secret)
        {
            if (
                string.IsNullOrEmpty(secret.Content) ||
                secret.Content.Contains("'") ||
                string.IsNullOrEmpty(secret.Title) ||
                secret.Title.Contains("'") ||
                string.IsNullOrEmpty(secret.OwnerId) ||
                secret.OwnerId.Contains("'")
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

            await _groupsService.DeleteGroupSecret(secretId);
            await _secretsService.Delete(secretId);
            return Ok("Secret deleted successfully!");
        }
    }
}