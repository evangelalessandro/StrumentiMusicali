using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageArticoloRemove : FilterControllerEvent
    {
		public ImageArticoloRemove(FotoArticolo fotoArticolo, interfaces.IKeyController controller)
            : base(controller)
        {
            FotoArticolo = fotoArticolo;
		}

		public FotoArticolo FotoArticolo { get; private set; }
	}
}