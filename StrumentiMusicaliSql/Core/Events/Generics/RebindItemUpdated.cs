using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
