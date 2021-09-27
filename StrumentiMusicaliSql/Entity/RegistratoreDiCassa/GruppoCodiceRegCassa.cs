using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.RegistratoreDiCassa
{
    public class GruppoCodiceRegCassa : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20, ErrorMessage = "Il nome deve essere univoco")]
        public string GruppoRaggruppamento { get; set; }


    }
}
