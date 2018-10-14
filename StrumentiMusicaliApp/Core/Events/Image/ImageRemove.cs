using StrumentiMusicaliSql.Entity;

namespace StrumentiMusicaliApp.Core.Events.Image
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
