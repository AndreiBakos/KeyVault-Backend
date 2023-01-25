using System.ComponentModel.DataAnnotations;

namespace KeyVault.Models.User
{
    public class UserForHome
    {
        [MaxLength(250)]
        public string UserId { get; set; }

        [MaxLength(250)] 
        public string UserName { get; set; }
        
        [MaxLength(250)]
        public string Email { get; set; }

        public UserForHome() {}

        public UserForHome(Entities.User user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}