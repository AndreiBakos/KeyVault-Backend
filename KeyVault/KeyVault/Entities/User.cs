using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
namespace KeyVault.Entities
{
    [Table("Users")]
    public class User
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string UserId { get; set; } 
        
        [MaxLength(250)]
        public string UserName { get; set; }
        
        [MaxLength(250)]
        public string Email { get; set; }
        
        [MaxLength(250)]
        public string Password { get; set; }
    }
}