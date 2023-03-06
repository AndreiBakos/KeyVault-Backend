using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using KeyVault.Entities;
using KeyVault.Models.GroupMember;
using KeyVault.Models.Groups;
using KeyVault.Models.GroupSecrets;
using KeyVault.Models.Secrets;
using KeyVault.Models.User;
using KeyVault.Services.Secrets;
using KeyVault.Tools;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.Services.Groups
{
    public class GroupsService: IGroupsService
    {
        private readonly IConfiguration _config;
        private readonly ISecretsService _secretsService;
        public GroupsService(IConfiguration config, ISecretsService secretsService)
        {
            _config = config;
            _secretsService = secretsService;
        }

        public async Task<GroupsForHome> Get(string id)
        {
            var query = @$"SELECT g.groupId, g.title, g.owner_id as ownerId                       
                                FROM `Group` as g INNER JOIN GroupMember gm ON g.groupId = gm.groupId
                                WHERE g.groupId = @groupId";

            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var group = await connection.QueryFirstOrDefaultAsync<GroupsForHome>(query, new { GroupId = id });
                if (group == null)
                {
                    return null;
                }
                group.Members = await GetMembers(id);
                return group;
            }
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

        public async Task InsertMember(List<GroupMemberForCreation> groupMemberForCreation)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                foreach (var group in groupMemberForCreation)
                {
                    var newGroupMembers = new GroupMember(group);
                    await connection.InsertAsync(newGroupMembers);
                }
            }
        }

        public async Task<GroupsForHome> Create(GroupForCreation group)
        {
            var newGroup = new Group(group);
            var insertGroupQuery = @$"insert into `Group` VALUES (
                            '{newGroup.GroupId}',
                            '{newGroup.Title}',
                            '{newGroup.OwnerId}')";

            var newGroupMember = new GroupMember(new GroupMemberForCreation(newGroup.GroupId, newGroup.OwnerId));
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(insertGroupQuery);
                await connection.InsertAsync(newGroupMember);

                var groupForHome = new GroupsForHome(newGroup, await GetMembers(newGroup.GroupId));
                return groupForHome;
            }
        }

        public async Task Delete(string groupId)
        {
            var query = $@"DELETE FROM `Group` WHERE groupId = '{groupId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var ownerSecrets = await GetGroupSecrets(groupId);
                await DeleteAllGroupSecrets(groupId);
                await DeleteGroupMember(groupId);
                await connection.ExecuteAsync(query);
                foreach (var secret in ownerSecrets)
                {
                    await _secretsService.Delete(secret.Id);
                }
            }
        }

        public async Task DeleteGroupMember(string groupId)
        {
            var query = $@"DELETE FROM `GroupMember` WHERE groupId = '{groupId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query);
            }
        }

        public async Task DeleteAllGroupSecrets(string groupId)
        {
            var query = $@"DELETE FROM `GroupSecret` WHERE group_id = '{groupId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query);
            }
        }

        public async Task<IEnumerable<SecretForHome>> GetGroupSecrets(string groupId)
        {
            var queryMembers = @$"SELECT s.secretId as Id, s.title, s.content, s.dateCreated, s.ownerId FROM Secret AS s
                                INNER JOIN GroupSecret gs ON s.secretId = gs.secretId
                                INNER JOIN `Group` g ON gs.group_id = g.groupId
                                WHERE g.groupId = '{groupId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var secrets = await connection.QueryAsync<SecretForHome>(queryMembers);
                foreach (var secret in secrets)
                {
                    secret.Content = new CryptoTool()
                        .Decrypt(
                            secret.Content,
                            Guid.Parse(_config["AppSettings:Key"]), 
                            Guid.Parse(_config["AppSettings:Iv"])
                        );
                }
                return secrets;
            }
        }

        public async Task<SecretForHome> CreateGroupSecret(GroupSecretsForCreation groupSecret)
        {
            var newSecret = await _secretsService.Create(groupSecret.Secret);
            var newGroupSecret = new GroupSecret(groupSecret, newSecret.SecretId);
            var query = $@"INSERT INTO GroupSecret VALUES ('{newGroupSecret.GroupSecretId}', '{newGroupSecret.GroupId}', '{newGroupSecret.SecretId}')";

            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query);

                
                return await _secretsService.FilterById(newSecret.SecretId);
            }
        }

        public async Task DeleteGroupSecret(string secretId)
        {
            var query = $@"DELETE FROM GroupSecret WHERE secretId = '{secretId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query);
            }
        }

        public async Task DeleteMember(List<string> memberIds)
        {

            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                foreach (var memberId in memberIds)
                {
                    await connection.ExecuteAsync($@"DELETE FROM GroupMember WHERE memberId = '{memberId}'");
                }
            }            
        }
    }
}