using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Altro
{
    public class CodiciABarre : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "Il valore deve essere univoco (*) qualsiasi")]
        public string CodiceABarre { get; set; }
        
        [MaxLength(50)]
        public string Azione { get; set; } = "";

        [MaxLength(50)]
        public string Descrizione { get; set; } = "";

        public int CodiceIva { get; set; } = -1;

    }
}
