using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Articoli
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