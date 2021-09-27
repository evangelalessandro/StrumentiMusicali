using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;

namespace StrumentiMusicali.Library.Core
{
    public class CategoriaItem : BaseItem<Categoria>
    {
        public CategoriaItem()
        {
        }

        public CategoriaItem(Categoria item)
        {
            ID = item.ID;
            Descrizione = item.Nome;
            Reparto = item.Reparto;
            if  (item.GruppoCodiceRegCassa!=null)
                GruppoRepartoCassa = item.GruppoCodiceRegCassa.GruppoRaggruppamento;

        }
        public string Descrizione { get; set; }

        public string Reparto { get; set; }
        public string GruppoRepartoCassa { get; }

        public override string ToString()
        {
            return Descrizione;
        }
    }
}