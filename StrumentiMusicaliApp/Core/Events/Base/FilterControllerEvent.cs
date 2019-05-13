using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Base
{
    public class FilterControllerEvent 
    {
        public FilterControllerEvent(interfaces baseController)
        {
            GuidKey = baseController._INSTANCE_KEY;
        }
        public Guid GuidKey { get; private set; }
    }
}
