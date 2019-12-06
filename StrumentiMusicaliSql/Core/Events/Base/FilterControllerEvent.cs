using StrumentiMusicali.Library.Core.interfaces;

namespace StrumentiMusicali.Library.Core.Events.Base
{
    public class FilterControllerEvent : FilterEvent
    {
        public FilterControllerEvent(IKeyController baseController)
            : base(baseController.INSTANCE_KEY)
        {

        }

    }
}
