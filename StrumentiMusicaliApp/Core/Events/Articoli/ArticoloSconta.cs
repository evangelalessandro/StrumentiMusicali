namespace StrumentiMusicali.App.Core.Events.Articoli
{
	public class ArticoloSconta
	{
		public ArticoloSconta(decimal valorePerc)
		{
			Percentuale = valorePerc;
		}
		public decimal Percentuale { get; private set; }
	}
}