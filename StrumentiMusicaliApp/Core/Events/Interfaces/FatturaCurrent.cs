using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events
{
	public class FatturaCurrent : CurrentItem<FatturaItem, Fattura>
	{
		public FatturaCurrent(FatturaItem itemSelected)
			: base(itemSelected)
		{
		}
	}
}