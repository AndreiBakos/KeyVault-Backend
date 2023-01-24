using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using KeyVault.Entities;
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
    }
}