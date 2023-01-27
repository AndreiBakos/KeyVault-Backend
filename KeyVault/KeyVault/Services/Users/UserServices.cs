using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using KeyVault.Entities;
using KeyVault.Models.User;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.Services.Users
{
    public class UserServices: IUserServices
    {
        private readonly IConfiguration _config;

        public UserServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = @"SELECT * FROM User";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var result = await connection.QueryAsync<User>(query);
                return result;
            }
        }

        public async Task<UserForHome> LoginUser(string email, string password)
        {
            var query = @"SELECT userId, userName, email FROM User WHERE email = @email AND password = @password";
            
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var result = await connection.QueryFirstOrDefaultAsync<UserForHome>(query, new { Email = email, Password = password });
                
                return result;
            }
        }

        public async Task<UserForHome> CreateUser(UserForCreation user)
        {
            var newUser = new User(user);
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.InsertAsync(newUser);
                
                var newUserForHome = new UserForHome(newUser);
                return newUserForHome;
            }
        }

        public async Task<IEnumerable<UserForHome>> FilterByUserName(string userName)
        {
            var query = $"SELECT userId as Id, userName, email FROM USER WHERE userName LIKE '{userName}%'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var result = await connection.QueryAsync<UserForHome>(query, new { UserName = userName });

                return result;
            }
        }
    }
}