using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core
{
	public class CategoriaItem
	{
		public string Descrizione { get; set; }
		public int ID { get; set; }
		public override string ToString()
		{
			return Descrizione;
		}
	}
}
