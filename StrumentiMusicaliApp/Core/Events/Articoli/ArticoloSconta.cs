using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;

namespace StrumentiMusicali.App.Core.Events.Articoli
{
	public class ArticoloSconta : FilterControllerEvent
    {
		public ArticoloSconta(decimal valorePerc, BaseController controller)
            : base(controller)
        { 
            Percentuale = valorePerc;
		}
		public decimal Percentuale { get; private set; }
	}
}