using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Fatture
{
    
    public class TipiPagamentoDocumenti : BaseEntity
    {
        [StringLength(5, ErrorMessage = "Descrizione non valida, deve essere da 3 a 50 caratteri", MinimumLength = 1)]
        public string PreCodice { get; set; }

        [StringLength(50, ErrorMessage = "Descrizione non valida, deve essere da 3 a 50 caratteri", MinimumLength = 3)]
        public string Descrizione { get; set; }

        [CustomUIView(Titolo ="Abilitato")]
        public bool Enable { get; set; } = true;

    }
}
