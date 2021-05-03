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
            
            Abilitato = item.Enable;
            Entity = item;
            ID = item.ID;
        } 

        public string Descrizione { get; set; }

        public bool Abilitato{ get; set; }

    }
}
