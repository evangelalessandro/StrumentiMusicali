using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Ecomm;

namespace StrumentiMusicali.EcommerceBaseSyncro
{

    public class ArticoloBase
    {
        public int ArticoloID { get; set; }
        public long CodiceArticoloEcommerce { get; set; }
        public Articolo ArticoloDb { get; set; }
        public AggiornamentoWebArticolo Aggiornamento { get; set; }

    }

}