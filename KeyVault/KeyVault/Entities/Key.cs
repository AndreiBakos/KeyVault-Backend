using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
namespace KeyVault.Entities
{
    [Dapper.Contrib.Extensions.Table("Key")]
    public class Key
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string KeyId { get; set; }
        
        [MaxLength(250)]
        public string message { get; set; }
        
        [MaxLength(250)]
        [ForeignKey("SecretId")]
        public string Secret_id { get; set; }
    }
}