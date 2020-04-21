using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.interfaces;

namespace StrumentiMusicali.Library.Core.Events.Articoli
{
    public class ArticoloMerge : FilterControllerEvent
    {
        public ArticoloMerge(IKeyController controller)
            : base(controller)
        {

        }
    }
}