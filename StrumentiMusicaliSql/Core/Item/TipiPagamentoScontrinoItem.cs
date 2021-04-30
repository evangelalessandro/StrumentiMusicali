
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Fatture;

namespace StrumentiMusicali.Library.Core.Item
{
    public class TipiPagamentoScontrinoItem : BaseItem<TipiPagamentoScontrino>
    {
        public TipiPagamentoScontrinoItem()
        {

        }
        public TipiPagamentoScontrinoItem(TipiPagamentoScontrino item)
        {
            Descrizione = item.Descrizione;
            Codice = item.Codice;
            Abilitato = item.Enable;
            Entity = item;
            ID = item.ID;
        }
        public int Codice { get; set; }

        public string Descrizione { get; set; }

        public bool Abilitato{ get; set; }

    }
}
