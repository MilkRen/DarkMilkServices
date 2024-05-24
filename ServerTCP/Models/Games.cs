using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerTCP.Models
{
    internal class Games
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public string descriptionENG { get; set; }
    }
}
