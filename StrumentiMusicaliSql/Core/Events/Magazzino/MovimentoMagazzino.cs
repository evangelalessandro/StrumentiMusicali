namespace StrumentiMusicali.Library.Core.Events.Magazzino
{
	public class MovimentoMagazzino
	{
		int _Deposito=0;
		public decimal Qta { get; set; }
		public int Deposito {
			get {
				return _Deposito; }
			set {
				_Deposito = value;
			}
		}
		public int ArticoloID { get; set; }
	}
}