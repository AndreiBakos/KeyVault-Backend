using System.ComponentModel.DataAnnotations;

namespace KeyVault.Models.Groups
{
    public class GroupForCreation
    {
        [MaxLength(250)]
        public string Title { get; set; }
        
        [MaxLength(250)]
        public string OwnerId { get; set; }
    }
}