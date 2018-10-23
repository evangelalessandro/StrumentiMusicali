using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Item.Base;

namespace StrumentiMusicali.App.Core.Events
{
	public class FatturaCurrent : CurrentItem<FatturaItem>
	{
		
		public FatturaCurrent(FatturaItem itemSelected)
			:base(itemSelected)
		{
			
		}
	}
}
