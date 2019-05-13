using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class Add<TEntity> : FilterControllerEvent 
        where TEntity : BaseEntity
	{
		public Add( BaseController controller)
            : base(controller)
        {
        }
	}
}