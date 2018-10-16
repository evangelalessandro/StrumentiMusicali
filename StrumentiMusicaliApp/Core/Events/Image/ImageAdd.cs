using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageAdd
	{
		public ImageAdd(Articolo articolo)
		{
			Articolo = articolo;
		}
		public Articolo Articolo { get; private set; }
	}
}
