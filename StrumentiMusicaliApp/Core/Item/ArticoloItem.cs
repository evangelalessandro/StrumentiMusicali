using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Properties;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.Core
{
	public class ArticoloItem : BaseItem<Articolo>
	{
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
				return new System.Drawing.Bitmap(20, 20);
			}
		}
	}
}