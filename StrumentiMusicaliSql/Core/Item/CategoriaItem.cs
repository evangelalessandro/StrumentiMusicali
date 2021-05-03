using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;

namespace StrumentiMusicali.Library.Core
{
    public class CategoriaItem : BaseItem<Categoria>
    {
        public CategoriaItem()
            : base()
        {
        }
        public CategoriaItem(Categoria item)
            : base()
        {
            Codice = item.Codice;
            Reparto = item.Reparto;
            CategoriaCondivisaCon = item.CategoriaCondivisaCon;
            Nome = item.Nome;
        }
        public int Codice { get; set; }
        public string Reparto { get; set; }

        public string Nome { get; set; }

        public string CategoriaCondivisaCon { get; set; }

        
    }
}