using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
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
    }
}