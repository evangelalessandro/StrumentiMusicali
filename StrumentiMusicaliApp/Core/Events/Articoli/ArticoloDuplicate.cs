using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;

namespace StrumentiMusicali.App.Core.Events.Articoli
{
	public class ArticoloDuplicate : FilterControllerEvent
    {
        public ArticoloDuplicate(  BaseController controller)
            : base(controller)
        {

        }
	}
}