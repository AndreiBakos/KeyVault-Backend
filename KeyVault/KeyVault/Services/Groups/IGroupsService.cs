using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.GroupMember;
using KeyVault.Models.Groups;
using KeyVault.Models.GroupSecrets;
using KeyVault.Models.Secrets;
using KeyVault.Models.User;

namespace KeyVault.Services.Groups
{
    public interface IGroupsService
    {
        Task<IEnumerable<GroupsForHome>> Filter(string userId);
        Task<IEnumerable<UserForHome>> GetMembers(string groupId);
        Task InsertMember(List<GroupMemberForCreation> groupMemberForCreation);
        Task<GroupsForHome> Create(GroupForCreation group);
        Task Delete(string groupId);
        Task DeleteGroupMember(string groupId);
        Task<IEnumerable<SecretForHome>> GetGroupSecrets(string groupId);
        Task<GroupSecret> CreateGroupSecret(GroupSecretsForCreation groupSecret);
        Task DeleteGroupSecret(string secretId);
        Task DeleteMember(List<string> memberIds);
    }
}