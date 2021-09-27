using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;

namespace StrumentiMusicali.Library.Core
{
    public class RegistratoreDiCassaRepartiItem : BaseItem<RegistratoreDiCassaReparti>
    {
        public RegistratoreDiCassaRepartiItem()
        {
        }

        public RegistratoreDiCassaRepartiItem(RegistratoreDiCassaReparti item)
        {
            ID = item.ID;
            Descrizione = item.NomeReparto;
            

        }
        public string Descrizione { get; set; }

        public override string ToString()
        {
            return Descrizione;
        }
    }
}