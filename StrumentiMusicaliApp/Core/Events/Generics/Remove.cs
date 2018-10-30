using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class Remove<TBaseItem, TEntity>
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public Remove()
		{
		}
	}
}