using StrumentiMusicali.Library.Core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core.Events.Base
{
    public class FilterControllerEvent 
    {
        public FilterControllerEvent(IKeyController baseController)
        {
            GuidKey = baseController.INSTANCE_KEY;
        }
        public Guid GuidKey { get; private set; }
    }
}
