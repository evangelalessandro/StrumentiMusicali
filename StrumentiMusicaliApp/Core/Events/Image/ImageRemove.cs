using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageRemove
	{
		public ImageRemove(FotoArticolo fotoArticolo)
		{
			FotoArticolo = fotoArticolo;
		}

		public FotoArticolo FotoArticolo { get; private set; }
	}
}