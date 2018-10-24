using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Properties;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core.Item
{
	public class FatturaRigaItem : BaseItem
	{
		
		public string CodiceArt { get; set; }
		
		public string Descrizione { get; set; }

		public int Qta { get; set; }

		public decimal PrezzoUnitario { get; set; }

		public decimal Importo { get; set; }

		public string Iva { get; set; }


		public FatturaRiga FatturaRigaCS { get; set; }

	}
}
