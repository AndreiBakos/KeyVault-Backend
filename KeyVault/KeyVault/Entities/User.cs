using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using KeyVault.Models.User;

namespace KeyVault.Entities
{
    [Table("User")]
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

        public User(UserForCreation user)
        {
            UserId = Guid.NewGuid().ToString();
            UserName = user.UserName;
            Email = user.Email;
            Password = user.Password;
        }
    }
}