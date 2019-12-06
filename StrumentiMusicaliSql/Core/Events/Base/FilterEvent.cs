using System;

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
