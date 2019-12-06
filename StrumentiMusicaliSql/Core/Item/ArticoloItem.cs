using StrumentiMusicali.Library.Core.Item.Base;

using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core
{
    public class ArticoloItem : BaseItem<Articolo>
    {
        public ArticoloItem()
        {

        }
        public ArticoloItem(Articolo articolo)
        {
            Titolo = articolo.Titolo;
            Marca = articolo.Strumento != null && articolo.Strumento.Marca != null ? articolo.Strumento.Marca : "";
            CodiceABarre = articolo.CodiceABarre;
            //CaricaInEcommerce = articolo.CaricainECommerce;
            //CaricaInMercatino = articolo.CaricaInMercatino;
            Categoria = articolo.Categoria.Nome;
            Reparto = articolo.Categoria.Reparto;
            ID = articolo.ID;
            Entity = articolo;
            //TagImport = articolo.TagImport;
            PrezzoAcquisto = articolo.PrezzoAcquisto.ToString("C2");
            Prezzo = articolo.Prezzo.ToString("C2");
            CodiceOrdine = articolo.Libro != null && articolo.Libro.Ordine != null ? articolo.Libro.Ordine :
                articolo.Strumento != null && articolo.Strumento.CodiceOrdine != null ? articolo.Strumento.CodiceOrdine : "";

            Settore = articolo.Libro != null && articolo.Libro.Settore != null
                ? articolo.Libro.Settore : "";

            //DataCreazione = articolo.DataCreazione;
            //DataModifica = articolo.DataUltimaModifica;
        }

        public string Reparto { get; set; }
        public string Categoria { get; set; }
        public string Titolo { get; set; }
        public string Marca { get; set; }
        public string Settore { get; set; }
        public string CodiceABarre { get; set; }
        public string CodiceOrdine { get; }
        //public string TagImport { get; }
        public string Prezzo { get; set; }
        public string PrezzoAcquisto { get; set; }
        public int QuantitaNegozio { get; set; }
        public int QuantitaTotale { get; set; }

    }
}