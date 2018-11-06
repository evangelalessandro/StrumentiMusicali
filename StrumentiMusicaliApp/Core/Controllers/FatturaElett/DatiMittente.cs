using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	public class DatiMittente : Cliente
	{
		//public string ProgressivoInvio { get; set; }

		[CustomUIViewAttribute(Ordine = 31)]
		public bool SoggettoARitenuta { get; set; }
		[CustomUIViewAttribute(Ordine = 32)]
		public bool IscrittoRegistroImprese { get; set; }
		[CustomUIViewAttribute(Ordine = 30)]
		public UfficioRegistro UfficioRegistroImp { get; set; } = new UfficioRegistro();

		public class UfficioRegistro
		{
			public string SiglaProv { get; set; }
			public string NumeroRea { get; set; }
			public decimal CapitaleSociale { get; set; }
			public bool SocioUnico { get; set; }
			public bool SocioMultiplo { get; set; }
		}
		/*assieme alla banca*/
		[CustomUIViewAttribute(Width = 120, Ordine = 13)]
		public string IBAN { get; set; }
	}
}