﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerTCP.Models
{
    internal class SalePrograms
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        public int program_id { get; set; }
    }
}