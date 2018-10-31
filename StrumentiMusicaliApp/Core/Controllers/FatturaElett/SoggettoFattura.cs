using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	public class SoggettoFattura
	{
		public string RagioneSociale { get; set; }
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public string RegimeFiscale { get; set; }
		public string IndirizzoConCivico { get; set; }
		public string Cap { get; set; }
		public string Comune { get; set; }
		public string ProvinciaSigla { get; set; }
		public string PIVA { get; set; }
		public bool PersonaGiuridica { get; set; }
		public string CodiceFiscale { get; set; }
	}
}
