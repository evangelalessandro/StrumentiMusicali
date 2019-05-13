using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Item.Base
{
	public class CurrentItem<TBaseItem, TEntity>:FilterControllerEvent
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public CurrentItem(TBaseItem itemSelected, IKeyController filterController)
            :base(filterController)
		{
			ItemSelected = itemSelected;
		}

		public TBaseItem ItemSelected { get; private set; }
	}
}