using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Properties;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core.Item
{
	public class FatturaItem : BaseItem
	{
		
		public string RagioneSociale { get; set; }
		public DateTime Data { get; set; }
		public string PIVA { get; set; }



		public Fattura FatturaCS { get; set; }

	}
}
