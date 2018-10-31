namespace StrumentiMusicali.Library.Entity
{
	public class Fattura : DocumentoFiscaleBase
	{
		public Fattura()
			: base()
		{
			TipoDocumento = 2;
			Pagamento = "";
		}

		public string Pagamento { get; set; }
		public decimal ImportoTotale { get; set; }
		public decimal TotaleIva { get; set; }
		public decimal TotaleFattura { get; set; }
	}
}