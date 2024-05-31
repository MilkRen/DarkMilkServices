using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerTCP.Models
{
    internal class SaleGames
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        public int game_id { get; set; }
    }
}
