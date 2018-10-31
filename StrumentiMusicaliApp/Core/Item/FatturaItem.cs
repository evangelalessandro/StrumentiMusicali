using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core.Item
{
	public class FatturaItem : BaseItem<Fattura>
	{
		public string RagioneSociale { get; set; }
		public DateTime Data { get; set; }
		public string PIVA { get; set; }

		public string Codice { get; set; }
	}
}