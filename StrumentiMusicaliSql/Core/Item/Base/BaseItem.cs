using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Item.Base
{
	public class BaseItem<TEntity> : BaseItemID
		where TEntity : BaseEntity
	{
		public TEntity Entity { get; set; }
	}
}