using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;
using KeyVault.Models.Secrets;

namespace KeyVault.Services.Secrets
{
    public interface ISecretsService
    {
        Task<IEnumerable<Secret>> GetSecrets(string ownerId);
        Task<Secret> Create(SecretForCreation secret);
        Task Delete(string secretId);
    }
}