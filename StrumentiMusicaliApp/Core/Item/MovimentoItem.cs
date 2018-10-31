using System;

namespace StrumentiMusicali.App.Core.Item
{
	internal class MovimentoItem
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