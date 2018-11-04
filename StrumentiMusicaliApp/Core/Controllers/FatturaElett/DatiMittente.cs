using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	public class DatiMittente : Cliente
	{
		public string ProgressivoInvio { get; set; }
		public bool VersoPA { get; set; }
		
		public bool SoggettoARitenuta { get; set; }
		public bool IscrittoRegistroImprese { get; set; }
		public UfficioRegistro UfficioRegistroImp { get; set; } = new UfficioRegistro();

		public class UfficioRegistro
		{
			public string SiglaProv { get; set; }
			public string NumeroRea { get; set; }
			public decimal CapitaleSociale { get; set; }
			public bool SocioUnico { get; set; }
			public bool SocioMultiplo { get; set; }
		}

		public string IBAN { get; set; }
	}
}