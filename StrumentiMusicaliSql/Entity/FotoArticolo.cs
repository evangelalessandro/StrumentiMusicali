using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliSql.Entity
{
    class FotoArticolo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public Articolo Articolo { get; set; }
        [Required]
        public string UrlFoto { get; set; }

    }
}
