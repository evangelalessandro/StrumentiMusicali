using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageAdd : FilterControllerEvent
    {
		public ImageAdd(Articolo articolo, interfaces.IKeyController controller)
            :base(controller)
		{
			Articolo = articolo;
		}

		public Articolo Articolo { get; private set; }
	}
}