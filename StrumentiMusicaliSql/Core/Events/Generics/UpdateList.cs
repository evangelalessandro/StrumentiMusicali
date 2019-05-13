using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
	public class UpdateList<TEntity>: FilterControllerEvent
        where TEntity : BaseEntity
	{
        public UpdateList(interfaces.IKeyController controller)
         : base(controller)
        {
        }
    }
}