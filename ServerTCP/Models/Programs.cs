using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerTCP.Models
{

    public class ProgramsForXml
    {
        public Programs[] Programs { get; set; }
    }

    public class Programs
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public string descriptionEng { get; set; }
    }
}
