using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class Add<TEntity> where TEntity : BaseEntity
	{
		public Add()
		{
		}
	}
}