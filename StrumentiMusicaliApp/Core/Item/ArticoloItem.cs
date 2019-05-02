using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Properties;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core
{
    public class ArticoloItem : BaseItem<Articolo>
    {
        public ArticoloItem()
        {

        }
        public ArticoloItem(Articolo articolo)
        {
            Titolo = articolo.Titolo;
            Marca = articolo.Marca;
            CodiceABarre = articolo.CodiceABarre;
            //CaricaInEcommerce = articolo.CaricainECommerce;
            //CaricaInMercatino = articolo.CaricaInMercatino;
            Categoria = articolo.Categoria.Nome;
            Reparto= articolo.Categoria.Reparto;
            ID = articolo.ID;
            Entity = articolo;
            //TagImport = articolo.TagImport;
            PrezzoAcquisto = articolo.PrezzoAcquisto.ToString("C2");
            Prezzo = articolo.Prezzo.ToString("C2");
            CodiceOrdine = articolo.Libro!=null && articolo.Libro.Ordine != null ? articolo.Libro.Ordine : "";
            //DataCreazione = articolo.DataCreazione;
            //DataModifica = articolo.DataUltimaModifica;
        }
        
        public string Reparto { get; set; }
        public string Categoria { get; set; }
        public string Titolo { get; set; }
        public string Marca { get; set; }
        public string CodiceABarre { get; set; }
        //public string TagImport { get; }
        public string Prezzo { get; set; }
        public string CodiceOrdine { get; }
        public string PrezzoAcquisto { get; set; }
        public int QuantitaNegozio { get; set; }
        public int QuantitaTotale { get; set; }
        //public bool CaricaInEcommerce { get; set; } 
        //public bool CaricaInMercatino { get; set; } 

        //public DateTime DataCreazione { get; set; }
        //public DateTime DataModifica { get; set; }

        //public bool Pinned { get; set; }

        //public System.Drawing.Bitmap PinnedImage {
        //	get {
        //		if (Pinned)
        //		{
        //			return Resources.pin_16;
        //		}
        //		return new System.Drawing.Bitmap(20, 20);
        //	}
        //}
    }
}