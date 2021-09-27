using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StrumentiMusicali.Library.Core;

namespace StrumentiMusicali.Library.Entity.RegistratoreDiCassa
{
    public class RegistratoreDiCassaReparti : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20, ErrorMessage = "Il nome deve essere univoco")]
        public string NomeReparto { get; set; }

        public int Iva { get; set; }


        public int CodicePerRegistratoreDiCassa { get; set; }


        [CustomUIView(Width = 350, Ordine = 1, Combo = TipoDatiCollegati.GruppoCodiceRegCassa, Titolo = "Gruppo Codice Registratore di Cassa")]
        [Required]
        public int GruppoCodiceRegCassaID { get; set; }

        [CustomHideUI]
        public virtual GruppoCodiceRegCassa GruppoCodiceRegCassa { get; set; }


    }
}
