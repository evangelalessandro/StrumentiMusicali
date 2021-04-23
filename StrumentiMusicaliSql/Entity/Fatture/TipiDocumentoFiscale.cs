using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class TipiDocumentoFiscale : BaseEntity
    {
        
        [Library.Core.CustomHideUI]
        public int IDEnum { get; set; }

        [StringLength(10,  ErrorMessage = "Codice non valido, deve essere da 3 a 50 caratteri", MinimumLength =1)]
        public string Codice { get; set; }

        [StringLength(50, ErrorMessage = "Descrizione non valida, deve essere da 3 a 50 caratteri", MinimumLength = 3)]
        public string Descrizione { get; set; }
         

    }
}
