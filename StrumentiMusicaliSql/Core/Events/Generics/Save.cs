using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
    public class Save<TEntity> : FilterControllerEvent
        where TEntity : BaseEntity
    {
        public Save(IKeyController controller)
         : base(controller)
        {
        }
    }
}
