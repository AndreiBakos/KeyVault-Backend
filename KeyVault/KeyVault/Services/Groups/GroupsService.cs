using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using KeyVault.Entities;
using KeyVault.Models.Groups;
using KeyVault.Models.User;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.Services.Groups
{
    public class GroupsService: IGroupsService
    {
        private readonly IConfiguration _config;

        public GroupsService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<GroupsForHome>> Filter(string userId)
        {
            var newGroupForHomeList = new List<GroupsForHome>();
            var queryGroups = @$"SELECT g.groupId, g.title, g.owner_id as ownerId                       
                                FROM `Group` as g INNER JOIN GroupMember gm ON g.groupId = gm.groupId
                                WHERE gm.memberId = '{userId}'";

            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var groups = await connection.QueryAsync<Group>(queryGroups);

                foreach (var group in groups)
                {
                    var members = await GetMembers(group.GroupId);
                    newGroupForHomeList.Add(new GroupsForHome(group, members));
                }

                return newGroupForHomeList;
            }
        }

        public async Task<IEnumerable<UserForHome>> GetMembers(string groupId)
        {
            var queryMembers = @$"SELECT u.userId as Id, u.userName, u.email FROM User AS u
                                INNER JOIN GroupMember gm ON u.userId = gm.memberId
                                INNER JOIN `Group` g ON gm.groupId = g.groupId
                                WHERE g.groupId = '{groupId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var members = await connection.QueryAsync<UserForHome>(queryMembers);

                return members;
            }
        }
    }
}