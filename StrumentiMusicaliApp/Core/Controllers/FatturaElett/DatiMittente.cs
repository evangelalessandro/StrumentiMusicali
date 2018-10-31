namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	public class DatiMittente : SoggettoFattura
	{

		public string Email { get; set; }
		public string ProgressivoInvio { get; set; }
		public bool VersoPA { get; set; }
		public string Telefono { get; internal set; }

		public bool SoggettoARitenuta { get; set; }
		public bool IscrittoRegistroImprese { get; set; }
		public UfficioRegistro UfficioRegistroImp { get; set; }
		public class UfficioRegistro
		{
			public string SiglaProv { get; set; }
			public string NumeroRea { get; set; }
			public decimal? CapitaleSociale { get; set; }
			public bool SocioUnico { get; set; }
			public bool SocioMultiplo { get; set; }
		}
		public string IBAN { get; set; }
	}

}
