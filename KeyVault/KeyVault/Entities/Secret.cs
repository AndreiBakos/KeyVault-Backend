using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
namespace KeyVault.Entities
{
    [Table("Secret")]
    public class Secret
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string SecretId { get; set; }
        
        [MaxLength(250)]
        public string Title { get; set; }
    
        [MaxLength(250)]
        public string Content { get; set; }
        
        [MaxLength(250)]
        public string DateCreated { get; set; }
        
        [MaxLength]
        public string OwnerId { get; set; }
    }
}