using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
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
        public string Group_id { get; set; }
        
        [ForeignKey("SecretId")]
        [MaxLength(250)]
        public string SecretId { get; set; }
    }
}