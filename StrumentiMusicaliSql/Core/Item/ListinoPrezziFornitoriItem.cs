using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;

namespace StrumentiMusicali.Library.Core.Item
{
    public class ListinoPrezziFornitoriItem : BaseItem<ListinoPrezziFornitori>
    {
        [CustomUIViewAttribute(Ordine = 2, Enable = false)]

        public int ArticoloID { get; set; }
        [CustomUIViewAttribute(Ordine = 1, Enable = false)]

        public int FornitoreID { get; set; }

        [CustomUIViewAttribute(Ordine = 3, Titolo = "Prezzo Unitario", Money = true,Enable =false)]
        public decimal Prezzo { get; set; } = 0;
        [CustomUIViewAttribute(Ordine = 5, Enable = false)]

        public string CodiceArticoloFornitore { get; set; } = "";
        [CustomUIViewAttribute(Ordine = 7,Titolo ="Qta giacenza attuale", Enable = false)]

        public int QtaGiacenza { get; set; }

        [CustomUIViewAttribute(Ordine = 8, Titolo = "Qta da ordinare", Enable = true)]

        public int QtaDaOrdinare { get; set; }

        [CustomUIViewAttribute(Ordine = 9, Titolo = "Qta Minima ordine", Enable = false)]
        public int QtaMinimaOrdine { get; set; }


    }
}