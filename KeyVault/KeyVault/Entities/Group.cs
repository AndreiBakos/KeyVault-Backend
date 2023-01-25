using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
namespace KeyVault.Entities
{
    [Dapper.Contrib.Extensions.Table("Group")]
    public class Group
    {
        [ExplicitKey]
        [MaxLength(250)]
        public string GroupId { get; set; }
        
        [MaxLength(250)]
        public string Title { get; set; }
        
        [MaxLength(250)]
        [ForeignKey("UserId")]
        public string User_id { get; set; }
    }
}