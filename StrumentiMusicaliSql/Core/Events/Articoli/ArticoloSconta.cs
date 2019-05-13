using StrumentiMusicali.Library.Core.Events.Base;

namespace StrumentiMusicali.Library.Core.Events.Articoli
{
    public class ArticoloSconta : FilterControllerEvent
    {
		public ArticoloSconta(decimal valorePerc, interfaces.IKeyController controller)
            : base(controller)
        { 
            Percentuale = valorePerc;
		}
		public decimal Percentuale { get; private set; }
	}
}