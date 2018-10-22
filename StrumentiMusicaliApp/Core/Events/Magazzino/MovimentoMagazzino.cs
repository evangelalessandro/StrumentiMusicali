using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Magazzino
{
	public class MovimentoMagazzino
	{
		public decimal Qta { get; set; }
		public int Deposito { get; set; }
		public string ArticoloID { get; set; }
	}
}
