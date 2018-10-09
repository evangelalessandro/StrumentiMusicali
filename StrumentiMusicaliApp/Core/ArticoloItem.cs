using StrumentiMusicaliSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SturmentiMusicaliApp.Core
{
	public class ArticoloItem
	{
		public Guid ID { get; set; }
		public string Titolo { get; set; }
		public DateTime DataCreazione { get; set; }
		public DateTime DataModifica { get; set; }
		public bool	Pinned { get; set; }

		public Articolo ArticoloCS { get; set; }

	}
}
