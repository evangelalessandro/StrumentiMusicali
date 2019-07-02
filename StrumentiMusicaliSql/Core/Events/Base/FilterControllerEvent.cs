using StrumentiMusicali.Library.Core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core.Events.Base
{
    public class FilterControllerEvent : FilterEvent
    {
        public FilterControllerEvent(IKeyController baseController)
            :base(baseController.INSTANCE_KEY)
        {
            
        }
        
    }
}
