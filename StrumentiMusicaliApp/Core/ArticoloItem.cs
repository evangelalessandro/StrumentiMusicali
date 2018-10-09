using StrumentiMusicaliSql.Entity;
using SturmentiMusicaliApp.Properties;
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
		public bool Pinned { get; set; }
		public System.Drawing.Bitmap PinnedImage {
			get {
				if (Pinned)
				{
					return Resources.pin_16;
				}
				return null;
			}
		} 
		

		public Articolo ArticoloCS { get; set; }

	}
}
