using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.GroupMember;
using KeyVault.Models.Groups;
using KeyVault.Models.GroupSecrets;
using KeyVault.Models.Secrets;
using KeyVault.Models.User;
using KeyVault.Services.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyVault.Controllers
{
    [Route("api/groups")]
    [ApiController]
    [Authorize]
    public class GroupsController: ControllerBase
    {
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupsForHome>>> Filter([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId) || userId.Contains(","))
            {
                return BadRequest("Invalid data provided");
            }

            var groups = await _groupsService.Filter(userId);

            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<GroupsForHome>> Create([FromBody] GroupForCreation group)
        {
            if (string.IsNullOrEmpty(group.Title) || string.IsNullOrEmpty(group.OwnerId))
            {
                return BadRequest("Invalid data provided!");
            }

            var newgroup = await _groupsService.Create(group);

            return Ok(newgroup);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string groupId)
        {
            if (string.IsNullOrEmpty(groupId) || groupId.Contains("'"))
            {
                return BadRequest("Invalid data provided");
            }

            await _groupsService.Delete(groupId);

            return Ok("Group deleted successfully");
        }

        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<UserForHome>>> GetMembers([FromQuery] string groupId)
        {
            if (string.IsNullOrEmpty(groupId) || groupId.Contains("'"))
            {
                return BadRequest("Invalid data provided!");
            }

            var members = await _groupsService.GetMembers(groupId);

            return Ok(members);
        }

        [HttpPost("members")]
        public async Task<ActionResult<IEnumerable<UserForHome>>> InsertMember([FromBody] List<GroupMemberForCreation> groupMemberForCreation)
        {
            var filteredList = new List<GroupMemberForCreation>();
            foreach (var group in groupMemberForCreation)
            {
                if (string.IsNullOrEmpty(group.GroupId) ||
                    string.IsNullOrEmpty(group.MemberId))
                {
                    return BadRequest("Invalid data provided!");
                }

                var members = await _groupsService.GetMembers(group.GroupId);
                var hasMember = members.Any(m => m.Id.Equals(group.MemberId));
                if (!hasMember)
                {
                    filteredList.Add(group);
                }
            }

            await _groupsService.InsertMember(filteredList);
            return Ok(filteredList);
        }

        [HttpDelete("members")]
        public async Task<ActionResult> DeleteMember([FromQuery] List<string> ids)
        {
            foreach (var memberId in ids)
            {
                if (string.IsNullOrEmpty(memberId) || memberId.Contains("'"))
                {
                    return BadRequest("Invalid data provided!");
                }
            }

            await _groupsService.DeleteMember(ids);

            return Ok("Members deleted successfully");
        }

        [HttpGet("secrets")]
        public async Task<ActionResult<IEnumerable<SecretForHome>>> GetGroupSecrets([FromQuery] string groupId)
        {
            if (string.IsNullOrEmpty(groupId) || groupId.Contains("'"))
            {
                return BadRequest("Invalid data provided!");
            }

            var secrets = await _groupsService.GetGroupSecrets(groupId);

            return Ok(secrets);
        }

        [HttpPost("secrets")]
        public async Task<ActionResult<Secret>> CreateGroupSecret(
            [FromBody] GroupSecretsForCreation groupSecretsForCreation)
        {
            if (string.IsNullOrEmpty(groupSecretsForCreation.GroupId) ||
                groupSecretsForCreation.GroupId.Contains("'") ||
                string.IsNullOrEmpty(groupSecretsForCreation.Secret.OwnerId) ||
                groupSecretsForCreation.Secret.OwnerId.Contains("'") ||
                string.IsNullOrEmpty(groupSecretsForCreation.Secret.Title) ||
                groupSecretsForCreation.Secret.Title.Contains("'") ||
                string.IsNullOrEmpty(groupSecretsForCreation.Secret.Content) ||
                groupSecretsForCreation.Secret.Content.Contains("'")
               )
            {
                return BadRequest("Invalid data provider!");
            }

            var groupSecret = await _groupsService.CreateGroupSecret(groupSecretsForCreation);

            return Ok(groupSecret);
        }

        [HttpDelete("secrets")]
        public async Task<ActionResult> DeleteGroupSecret([FromQuery] string secretId)
        {
            if (string.IsNullOrEmpty(secretId) || secretId.Contains("'"))
            {
                return BadRequest("Invalid data provided!");
            }

            await _groupsService.DeleteGroupSecret(secretId);

            return Ok("Group secret deleted successfully");
        }
    }
}