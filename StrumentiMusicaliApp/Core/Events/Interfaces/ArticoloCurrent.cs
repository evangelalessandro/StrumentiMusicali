using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;

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