using System.ComponentModel.DataAnnotations;

namespace KeyVault.Models.Secrets
{
    public class SecretForHome
    {
        [MaxLength(250)]
        public string Id { get; set; }
        
        [MaxLength(250)]
        public string Title { get; set; }
    
        [MaxLength(250)]
        public string Content { get; set; }
        
        [MaxLength(250)]
        public string DateCreated { get; set; }
        
        [MaxLength(250)]
        public string OwnerId { get; set; }

    }
}