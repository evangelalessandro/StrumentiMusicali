using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;
using System.Collections.Generic;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageAddFiles : FilterControllerEvent
    {
		public ImageAddFiles(Articolo articolo, List<string> files, interfaces.IKeyController controller)
            : base(controller)
        {
            Articolo = articolo;
			Files = files;
		}

		public Articolo Articolo { get; private set; }
		public List<string> Files { get; private set; }
	}
}