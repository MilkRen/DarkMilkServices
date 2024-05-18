using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerTCP.Models
{
    internal class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string username { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
