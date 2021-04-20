using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class Fattura : DocumentoFiscaleBase
    {
        public Fattura()
            : base()
        {
            TipoDocumento = EnTipoDocumento.NonSpecificato;
            Pagamento = "";
        }

        public bool ChiusaSpedita { get; set; } = false;
        public string Pagamento { get; set; }
        public decimal ImponibileIva { get; set; }
        public decimal TotaleIva { get; set; }
        public decimal TotaleFattura { get; set; }
        [Range(1, 100,
        ErrorMessage = "Occorre specificare il tipo documento")]
        public EnTipoDocumento TipoDocumento { get; set; }

        public virtual ICollection<FatturaRiga> RigheFattura { get; set; }
    }
}