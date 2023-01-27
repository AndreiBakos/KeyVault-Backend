using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Models.GroupMember;
using KeyVault.Models.Groups;
using KeyVault.Models.User;

namespace KeyVault.Services.Groups
{
    public interface IGroupsService
    {
        Task<IEnumerable<GroupsForHome>> Filter(string userId);
        Task<IEnumerable<UserForHome>> GetMembers(string groupId);
        Task InsertMember(List<GroupMemberForCreation> groupMemberForCreation);
    }
}