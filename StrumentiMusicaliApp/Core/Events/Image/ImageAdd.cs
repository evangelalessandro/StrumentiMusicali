using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageAdd : FilterControllerEvent
    {
		public ImageAdd(Articolo articolo, BaseController controller)
            :base(controller)
		{
			Articolo = articolo;
		}

		public Articolo Articolo { get; private set; }
	}
}