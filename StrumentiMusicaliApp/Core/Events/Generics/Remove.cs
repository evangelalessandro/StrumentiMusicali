using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class Remove<TEntity> where TEntity : BaseEntity
	{
		public Remove()
		{
		}
	}
}