using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class ItemSelected<TBaseItem, TEntity>: CurrentItem<TBaseItem,TEntity>
		where TBaseItem : BaseItem<TEntity>
		where TEntity : BaseEntity
	{
		public ItemSelected(TBaseItem baseItem)
			:base(baseItem)
		{

		}
	}
}