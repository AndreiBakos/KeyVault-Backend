using System.Collections.Generic;
using System.Threading.Tasks;
using KeyVault.Entities;

namespace KeyVault.Services.Keys
{
    public interface IKeysService
    {
        Task<IEnumerable<Key>> GetKeys(string secretId);
    }
}