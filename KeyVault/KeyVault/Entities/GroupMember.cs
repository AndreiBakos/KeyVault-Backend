using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using KeyVault.Models.GroupMember;

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
        public string GroupId { get; set; }
        
        [ForeignKey("UserId")]
        [MaxLength(250)]
        public string MemberId { get; set; }

        public GroupMember() { }

        public GroupMember(GroupMemberForCreation group)
        {
            GroupMemberId = Guid.NewGuid().ToString();
            GroupId = group.GroupId;
            MemberId = group.MemberId;
        }
    }
}