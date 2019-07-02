using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core.Events.Base
{
    public class FilterEvent
    {
        public FilterEvent(Guid guidKey)
        {
            GuidKey = guidKey;
        }
        public Guid GuidKey { get; private set; }
    }
}
