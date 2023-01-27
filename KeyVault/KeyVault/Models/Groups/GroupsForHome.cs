using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KeyVault.Entities;
using KeyVault.Models.Secrets;
using KeyVault.Models.User;
using Microsoft.AspNetCore.DataProtection;

namespace KeyVault.Models.Groups
{
    public class GroupsForHome
    {
        [MaxLength(250)]
        public string GroupId { get; }
        
        [MaxLength(250)]
        public string Title { get; }
        
        [MaxLength(250)]
        public string OwnerId { get; }
        
        public IEnumerable<UserForHome> Members { get; set; }

        public GroupsForHome() { }

        public GroupsForHome(Group group, IEnumerable<UserForHome> members)
        {
            GroupId = group.GroupId;
            Title = group.Title;
            OwnerId = group.OwnerId;
            Members = members;
        }
    }
}