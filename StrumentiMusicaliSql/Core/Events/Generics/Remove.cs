
using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
    public class Remove<TEntity> : FilterControllerEvent
        where TEntity : BaseEntity
    {
        public Remove(interfaces.IKeyController controller)
          : base(controller)
        {
        }
    }
}