using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using KeyVault.Models.Groups;

namespace KeyVault.Entities
{
    [Dapper.Contrib.Extensions.Table("Group")]
    public class Group
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string GroupId { get; set; }
        
        [MaxLength(250)]
        public string Title { get; set; }
        
        [MaxLength(250)]
        [ForeignKey("UserId")]
        public string OwnerId { get; set; }

        public Group() { }

        public Group(GroupForCreation group)
        {
            GroupId = Guid.NewGuid().ToString();
            Title = group.Title;
            OwnerId = group.Title;
        }
    }
}