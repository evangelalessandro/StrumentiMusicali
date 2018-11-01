using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;
using System.Drawing;

namespace StrumentiMusicali.App.Core.Item
{
	public class ClientiItem : BaseItem<Cliente>
	{
		public ClientiItem()
			: base()
		{
		}
		public ClientiItem(Cliente item)
			: base()
		{
			ID = item.ID.ToString();

			 
			PIVA= item.PIVA;
			RagioneSociale= item.RagioneSociale;
			Via= item.Via;
			DataCreazione = item.DataCreazione;
			Citta= item.Citta;
			Telefono= item.Telefono;
			Fax= item.Fax;
			Cellulare= item.Cellulare;

		}

		public string PIVA { get; set; }
		public string RagioneSociale { get; set; }

		public string Via { get; set; }
		public string Citta { get; set; }
		public string Telefono { get; set; }
		public string Fax { get; set; }
		public string Cellulare { get; set; }

		public DateTime DataCreazione { get; set; }
	}
}
