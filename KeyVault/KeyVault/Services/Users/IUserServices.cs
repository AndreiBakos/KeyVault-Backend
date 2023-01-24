using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;

namespace KeyVault.Services.Users
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetUsers();
    }
}