using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Ecomm;

namespace StrumentiMusicali.PrestaShopSyncro
{

    internal class ArticoloBase
    {
        public int ArticoloID { get; set; }
        public string CodiceArticoloEcommerce { get; set; }
        public Articolo ArticoloDb { get; set; }
        public AggiornamentoWebArticolo Aggiornamento { get; set; }

    }





}