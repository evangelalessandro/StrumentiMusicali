using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageRemove : FilterControllerEvent
    {
		public ImageRemove(FotoArticolo fotoArticolo, BaseController controller)
            : base(controller)
        {
            FotoArticolo = fotoArticolo;
		}

		public FotoArticolo FotoArticolo { get; private set; }
	}
}