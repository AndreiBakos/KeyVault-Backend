using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using KeyVault.Entities;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.Services.Keys
{
    public class KeysService: IKeysService
    {
        private readonly IConfiguration _config;

        public KeysService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Key>> GetKeys(string secretId)
        {
            var query = $"SELECT * FROM Key WHERE secretId = '{secretId}'";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var result = await connection.QueryAsync<Key>(query, new {SecretId = secretId});

                return result;
            }
        }
    }
}