using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;

namespace StrumentiMusicali.Library.Core.Item
{
    public class ListinoPrezziFornitoriItem : BaseItem<ListinoPrezziFornitori>
    {

        public string Fornitore { get; set; }
        public decimal Prezzo { get; set; } = 0;

        public string CodiceArticoloFornitore { get; set; } = "";
        public int MinimoOrdinabile { get; set; } = 0;
    }
}