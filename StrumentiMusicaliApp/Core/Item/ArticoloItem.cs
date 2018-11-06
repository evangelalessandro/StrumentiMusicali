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
			CodiceAbarre = articolo.CodiceAbarre;
			CaricainEcommerce = articolo.CaricainEcommerce;
			CaricainMercatino = articolo.CaricainMercatino;
			Categoria = articolo.Categoria.Nome;
			Reparto= articolo.Categoria.Reparto;
			ID = articolo.ID;
			Entity = articolo;
			Prezzo = articolo.Prezzo.ToString("C2");
			DataCreazione = articolo.DataCreazione;
			DataModifica = articolo.DataUltimaModifica;
		}
		
		public string Reparto { get; set; }
		public string Categoria { get; set; }
		public string Titolo { get; set; }
		public string Marca { get; set; }
		public string CodiceAbarre { get; set; }
		public string Prezzo { get; set; }
		public bool CaricainEcommerce { get; set; } 
		public bool CaricainMercatino { get; set; } 

		public DateTime DataCreazione { get; set; }
		public DateTime DataModifica { get; set; }
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