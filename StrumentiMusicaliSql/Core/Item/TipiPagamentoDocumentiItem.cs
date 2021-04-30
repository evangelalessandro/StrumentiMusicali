using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Fatture;

namespace StrumentiMusicali.Library.Core.Item
{
    public class TipiPagamentoDocumentiItem : BaseItem<TipiPagamentoDocumenti>
    {
        public TipiPagamentoDocumentiItem()
        {

        }
        public TipiPagamentoDocumentiItem(TipiPagamentoDocumenti item)
        {
            Descrizione = item.Descrizione;
            PreCodice = item.PreCodice;
            Abilitato = item.Enable;
            Entity = item;
            ID = item.ID;
        }
        public string PreCodice { get; set; }

        public string Descrizione { get; set; }

        public bool Abilitato{ get; set; }

    }
}
