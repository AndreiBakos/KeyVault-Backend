using System.ComponentModel.DataAnnotations;
using KeyVault.Models.Secrets;

namespace KeyVault.Models.GroupSecrets
{
    public class GroupSecretsForCreation
    {
        [MaxLength(250)]
        public string GroupId { get; set; }

        public SecretForCreation Secret { get; set; }
    }
}