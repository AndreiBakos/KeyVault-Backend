using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using KeyVault.Models.Secrets;

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
        
        [MaxLength(250)]
        public string OwnerId { get; set; }

        public Secret() { }
        public Secret(SecretForCreation secret)
        {
            SecretId = Guid.NewGuid().ToString();
            Title = secret.Title;
            Content = secret.Content;
            DateCreated = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
            OwnerId = secret.OwnerId;
        }
    }
}