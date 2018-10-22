using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item
{
	class MovimentoItem
	{
		public MovimentoItem()
		{

		}
		public int ID { get; set; }
		public string NomeDeposito { get; set; }
		public int Qta { get; set; }
		public DateTime Data { get; set; }
		public string Articolo { get; set; }

	}
}
