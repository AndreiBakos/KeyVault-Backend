using System.ComponentModel.DataAnnotations;

namespace KeyVault.Models.Secrets
{
    public class SecretForCreation
    {
        [MaxLength(250)] 
        public string Title { get; set; }
        
        [MaxLength(250)]
        public string Content { get; set; }
        
        [MaxLength(250)]
        public string OwnerId { get; set; }
    }
}