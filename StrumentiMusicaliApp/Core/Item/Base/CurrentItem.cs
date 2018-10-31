using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Item.Base
{
	public class CurrentItem<TBaseItem, TEntity>
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public CurrentItem(TBaseItem itemSelected)
		{
			ItemSelected = itemSelected;
		}

		public TBaseItem ItemSelected { get; private set; }
	}
}