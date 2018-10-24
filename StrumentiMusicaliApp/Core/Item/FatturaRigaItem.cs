using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Properties;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core.Item
{
	public class FatturaRigaItem : BaseItem<FatturaRiga>
	{
		
		public string CodiceArt { get; set; }
		
		public string RigaDescrizione { get; set; }

		public int RigaQta { get; set; }

		public decimal PrezzoUnitario { get; set; }

		public decimal RigaImporto { get; set; }

		public string Iva { get; set; }


		 
	}
}
