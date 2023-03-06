using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using KeyVault.Entities;
using KeyVault.Models.Secrets;
using KeyVault.Tools;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KeyVault.Services.Secrets
{
    public class SecretsService: ISecretsService
    {
        private readonly IConfiguration _config;

        public SecretsService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<SecretForHome> FilterById(string secretId)
        {
            var query = @"SELECT * FROM Secret WHERE secretId = @secretId";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var secret = await connection.QueryFirstOrDefaultAsync<Secret>(query, new {SecretId = secretId});

                secret.Content = new CryptoTool().Decrypt(
                    secret.Content,
                    Guid.Parse(_config["AppSettings:Key"]),
                    Guid.Parse(_config["AppSettings:Iv"]));

                return new SecretForHome()
                {
                    Id = secret.SecretId,
                    Title = secret.Title,
                    Content = secret.Content,
                    DateCreated = secret.DateCreated,
                    OwnerId = secret.OwnerId
                };
            }
        }

        public  async Task<IEnumerable<SecretForHome>> GetSecrets(string ownerId)
        {
            var query = $"SELECT secretId as Id, title, content, dateCreated, ownerId  FROM Secret WHERE ownerId = @ownerId ORDER BY dateCreated DESC";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                var secrets = await connection.QueryAsync<SecretForHome>(query, new { OwnerId = ownerId });

                foreach (var secret in secrets)
                {
                    secret.Content = new CryptoTool()
                        .Decrypt(
                            secret.Content,
                            Guid.Parse(_config["AppSettings:Key"]), 
                            Guid.Parse(_config["AppSettings:Iv"])
                            );
                    var indexAtFirstWhiteSpace = secret.DateCreated.IndexOf(" ", StringComparison.Ordinal);
                    secret.DateCreated = secret.DateCreated.Substring(0, indexAtFirstWhiteSpace);
                }
                return secrets;
            }
        }

        public async Task<Secret> Create(SecretForCreation secret)
        {
            secret.Content =
                new CryptoTool()
                    .Encrypt(
                        secret.Content,
                        Guid.Parse(_config["AppSettings:Key"]), 
                        Guid.Parse(_config["AppSettings:Iv"])
                        );
            var newSecret = new Secret(secret);
            var query =
                $"INSERT INTO Secret VALUES('{newSecret.SecretId}', '{newSecret.Title}', '{newSecret.Content}', '{newSecret.DateCreated}', '{newSecret.OwnerId}')";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query);
                
                newSecret.Content = new CryptoTool()
                    .Decrypt(
                        newSecret.Content,
                        Guid.Parse(_config["AppSettings:Key"]), 
                        Guid.Parse(_config["AppSettings:Iv"])
                    );
                return newSecret;
            }
        }

        public async Task Delete(string secretId)
        {
            var query = "DELETE FROM Secret WHERE secretId = @secretId";
            using (var connection = new MySqlConnection(_config.GetConnectionString("KeyVaultDb")))
            {
                await connection.ExecuteAsync(query, new {SecretId = secretId});
            }
        }
    }
}