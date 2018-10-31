namespace StrumentiMusicali.App.Core.Events.Magazzino
{
	public class MovimentoMagazzino
	{
		public decimal Qta { get; set; }
		public int Deposito { get; set; }
		public string ArticoloID { get; set; }
	}
}