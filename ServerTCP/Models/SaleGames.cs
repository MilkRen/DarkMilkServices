using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerTCP.Models
{

    public class SaleGamesForXml
    {
        public SaleGames[] SaleGamesArray { get; set; }
    }

    public class SaleGames
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        public int game_id { get; set; }
    }
}
