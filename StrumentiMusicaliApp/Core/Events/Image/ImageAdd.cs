using StrumentiMusicaliSql.Entity;

namespace StrumentiMusicaliApp.Core.Events.Image
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
