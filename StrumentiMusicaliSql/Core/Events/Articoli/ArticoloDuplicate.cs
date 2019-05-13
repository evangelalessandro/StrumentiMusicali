using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.interfaces;

namespace StrumentiMusicali.Library.Core.Events.Articoli
{
    public class ArticoloDuplicate : FilterControllerEvent
    {
        public ArticoloDuplicate(IKeyController controller)
            : base(controller)
        {

        }
	}
}