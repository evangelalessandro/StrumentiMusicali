using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
    public class RebindItemUpdated<TEntity> : FilterControllerEvent
        where TEntity : BaseEntity
    {
        public RebindItemUpdated(interfaces.IKeyController controller)
         : base(controller)
        {
        }
    }
}
