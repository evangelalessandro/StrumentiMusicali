using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity
{
	public class FattureGenerateInvio : BaseEntity
	{
		public virtual Fattura Fattura { get; set; }

		public int FatturaID { get; set; }

		public int ProgressivoInvio { get; set; }

		public string NomeFile { get; set; }

		public string FileInviato { get; set; }

	}
}
