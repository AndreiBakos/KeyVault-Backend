using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using KeyVault.Models.GroupSecrets;

namespace KeyVault.Entities
{
    [Dapper.Contrib.Extensions.Table("GroupSecret")]
    public class GroupSecret
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string GroupSecretId { get; set; }
        
        [ForeignKey("GroupId")]
        [MaxLength(250)]
        public string GroupId { get; set; }
        
        [ForeignKey("SecretId")]
        [MaxLength(250)]
        public string SecretId { get; set; }

        public GroupSecret() { }
        public GroupSecret(GroupSecretsForCreation group, string secretId)
        {
            GroupSecretId = Guid.NewGuid().ToString();
            GroupId = group.GroupId;
            SecretId = secretId;
        }
    }
}