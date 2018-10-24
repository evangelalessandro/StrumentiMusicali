using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events
{
	 
	public class ArticoloCurrent : CurrentItem<ArticoloItem, Articolo>
	{
		public ArticoloCurrent(ArticoloItem baseItem)
			: base(baseItem)
		{

		}
	}
}
