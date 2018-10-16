using StrumentiMusicali.Library.Entity;
using System.Collections.Generic;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageAddFiles
	{
		public ImageAddFiles(Articolo articolo,List<string> files)
		{
			Articolo = articolo;
			Files = files;
		}
		public Articolo Articolo { get; private set; }
		public List<string> Files { get; private set; }
	}
}
