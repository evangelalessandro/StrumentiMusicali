using StrumentiMusicali.App.Core.Item.Base;

namespace StrumentiMusicali.App.Core.Events
{
	public class ArticoloCurrent : CurrentItem<ArticoloItem>
	{
		
		public ArticoloCurrent(ArticoloItem itemSelected)
			:base(itemSelected)
		{
			
		}
	}
}
