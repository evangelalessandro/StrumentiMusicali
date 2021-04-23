using StrumentiMusicali.Library.Core.Attributes;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Item
{
    public class FatturaRigaItem : BaseItem<FatturaRiga>
    {
        [CustomHideUI]
        public EnTipoDocumento TipoDoc { get; set; }

        [CustomUIViewAttribute(Ordine = 1, Titolo = "Descrizione")]

        public string RigaDescrizione { get; set; }

        [CustomUIViewAttribute(Ordine = 2, Titolo = "Qta")]
        public int RigaQta { get; set; }

        [CustomUIViewAttribute(Ordine = 3, Titolo = "Prezzo Unitario", Money = true)]
        public decimal PrezzoUnitario { get; set; }

        [CustomUIViewAttribute(Ordine = 4, Titolo = "Importo riga", Money = true)]
        public decimal RigaImporto { get; set; }

        [CustomUIViewAttribute(Ordine = 5, Titolo = "Iva")]
        public string Iva { get; set; }



        [CustomFattureAttribute(TipoDocShowOnly = EnTipoDocumento.OrdineAlFornitore)]
        [CustomUIViewAttribute(Width = 40, Ordine = 6)]
        public string CodiceFornitore { get; set; } = "";

        [CustomFattureAttribute(TipoDocShowOnly = EnTipoDocumento.OrdineAlFornitore)]
        [CustomUIViewAttribute(Width = 40, Ordine = 7)]
        public int Evasi { get; set; } = 0;

        [CustomFattureAttribute(TipoDocShowOnly = EnTipoDocumento.OrdineDiCarico)]
        [CustomUIViewAttribute(Width = 40, Ordine = 8)]
        public int Ricevuti { get; set; } = 0;

    }
}