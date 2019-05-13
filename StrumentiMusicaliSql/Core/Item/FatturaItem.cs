using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
	public class FatturaItem : BaseItem<Fattura>
	{

		public string Codice { get; set; }
		public DateTime Data { get; set; }
		public string TipoDocumento { get; set; }
		public string RagioneSociale { get; set; }
		public string PIVA { get; set; }

		
	}
}