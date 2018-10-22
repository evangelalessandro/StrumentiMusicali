using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item
{
	class DepositoItem
	{
		public string Descrizione => this.ToString();
		public int ID { get; set; }
		public int Qta { get; set; } = 0;
		public string NomeDeposito { get; set; }

		public override string ToString()
		{
			return NomeDeposito + " Qta:" + Qta.ToString() ; 
		}

	}
}
