using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
    public class FatturaRiga : RigaDoc
    {
        [CustomHideUIAttribute]
        [Required]
        public int FatturaID { get; set; }

        [CustomHideUIAttribute]
        public virtual Fattura Fattura { get; set; }

        [CustomHideUIAttribute]
        [NotMapped]
        public decimal Importo { get { return PrezzoUnitario * Qta; } set { } }

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 4, Money = true)]
        public decimal PrezzoUnitario { get; set; }
        [CustomUIViewAttribute(Width = 40, Ordine = 5)]
        public string IvaApplicata { get; set; }


        [CustomFattureAttribute(TipoDocShowOnly = EnTipoDocumento.OrdineAlFornitore)]
        [CustomUIViewAttribute(Width = 40, Ordine = 10)]
        public string CodiceFornitore { get; set; } = "";

        [CustomFattureAttribute(TipoDocShowOnly = EnTipoDocumento.OrdineAlFornitore)]
        [CustomUIViewAttribute(Width = 40, Ordine = 11)]
        public int Evasi { get; set; } = 0;

    }
}