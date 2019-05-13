using System;

namespace StrumentiMusicali.Library.Core.Item
{
	public class MovimentoItem
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