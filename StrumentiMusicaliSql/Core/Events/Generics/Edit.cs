using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
	public class Edit<TEntity> : FilterControllerEvent 
        where TEntity : BaseEntity
	{
        public Edit(interfaces.IKeyController controller)
           : base(controller)
        {
        }
    }
}