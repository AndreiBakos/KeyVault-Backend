using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.GroupMember;
using KeyVault.Models.Groups;
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

        [HttpPost("members")]
        public async Task<ActionResult> InsertMember([FromBody] List<GroupMemberForCreation> groupMemberForCreation)
        {
            var filteredList = new List<GroupMemberForCreation>();
            foreach (var group in groupMemberForCreation)
            {
                if (string.IsNullOrEmpty(group.GroupId) ||
                    string.IsNullOrEmpty(group.MemberId))
                {
                    return BadRequest("Invalid data provided!");
                }

                var members = await _groupsService.GetMembers(group.MemberId);
                var hasMember = members.Any(m => m.Id.Equals(group.MemberId));
                if (!hasMember)
                {
                    filteredList.Add(group);
                }
            }

            await _groupsService.InsertMember(filteredList);
            return Ok($"{groupMemberForCreation.Count} members added successfully");
        }
    }
}