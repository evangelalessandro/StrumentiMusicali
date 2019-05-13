 
using StrumentiMusicali.Library.Core.Events.Base;
 
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
	public class Add<TEntity> : FilterControllerEvent 
        where TEntity : BaseEntity
	{
		public Add(interfaces.IKeyController controller)
            : base(controller)
        {
        }
	}
}