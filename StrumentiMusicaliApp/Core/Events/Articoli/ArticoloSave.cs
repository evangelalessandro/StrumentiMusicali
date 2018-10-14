using StrumentiMusicaliSql.Entity;

namespace StrumentiMusicaliApp.Core.Events.Articoli
{
	public class ArticoloSave
	{
		public ArticoloSave(Articolo articolo)
		{
			Articolo = articolo;
		}
		public Articolo Articolo { get; private set; }
	}
}
