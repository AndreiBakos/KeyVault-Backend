using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.User;

namespace KeyVault.Services.Users
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetUsers();
        Task<UserForHome> LoginUser(string email, string password);
        Task<UserForHome> CreateUser(UserForCreation user);
    }
}