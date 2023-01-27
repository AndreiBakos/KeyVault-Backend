using System.ComponentModel.DataAnnotations;

namespace KeyVault.Models.User
{
    public class UserForCreation
    {
        [MaxLength(250)] 
        public string UserName { get; set; }
        
        [MaxLength(250)]
        public string Email { get; set; }
     
        [MaxLength(250)]
        public string Password { get; set; }
    }
}