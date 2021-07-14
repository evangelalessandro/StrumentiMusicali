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
    public class Postazione : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20, ErrorMessage = "Il nome deve essere univoco")]
        public string NomePostazione { get; set; }
        public int Versione { get; set; } = 0;

        public bool ForceUpdate { get; set; } = false;

        public string Note { get; set; } = "";


    }
}
