using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
namespace KeyVault.Entities
{
    [Dapper.Contrib.Extensions.Table("GroupMember")]
    public class GroupMember
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string GroupMemberId { get; set; }
        
        [ForeignKey("GroupId")]
        [MaxLength(250)]
        public string Group_id { get; set; }
        
        [ForeignKey("UserId")]
        [MaxLength(250)]
        public string MemberId { get; set; }
    }
}