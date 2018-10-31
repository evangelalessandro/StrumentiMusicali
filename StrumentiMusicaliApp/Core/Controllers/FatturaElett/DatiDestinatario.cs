using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	public class DatiDestinatario : SoggettoFattura
	{
		public string CodicePEC { get; set; }
		public bool RicezioneConCodicePec { get; set; }
		public string EmailPEC { get; set; }


	}
}
