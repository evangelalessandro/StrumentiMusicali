using StrumentiMusicaliApp.Properties;
using StrumentiMusicaliSql.Entity;
using System;

namespace StrumentiMusicaliApp.Core
{
	public class ArticoloItem
	{
		public string ID { get; set; }
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
				return new System.Drawing.Bitmap(20,20);
			}
		}


		public Articolo ArticoloCS { get; set; }

	}
}
