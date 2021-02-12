using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Entity.Ecomm
{
    public class CategoriaWeb : BaseEntity
    {

        public int CategoriaID { get; set; }

        public virtual Categoria Categoria { get; set; }
        public long CodiceWeb { get; set; }
        
    }
}