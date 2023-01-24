using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.DbContext
{
    public class KeyVaultContext
    {
        public MySqlConnection Connection { get; }

        public KeyVaultContext(IConfiguration config)
        {
            Connection = new MySqlConnection(config.GetConnectionString("KeyVault"));
        }
    }
}