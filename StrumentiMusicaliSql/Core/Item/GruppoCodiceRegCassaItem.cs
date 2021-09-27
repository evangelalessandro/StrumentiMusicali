using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;

namespace StrumentiMusicali.Library.Core
{
    public class GruppoCodiceRegCassaItem : BaseItem<GruppoCodiceRegCassa>
    {
        public GruppoCodiceRegCassaItem()
        {
        }

        public GruppoCodiceRegCassaItem(GruppoCodiceRegCassa item)
        {
            ID = item.ID;
            Descrizione = item.GruppoRaggruppamento;
            

        }
        public string Descrizione { get; set; }

        public override string ToString()
        {
            return Descrizione;
        }
    }
}