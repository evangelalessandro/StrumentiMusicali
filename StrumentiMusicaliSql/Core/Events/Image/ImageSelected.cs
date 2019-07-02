using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageSelected : FilterEvent
    {
		public ImageSelected(string file, System.Guid key)
            : base(key)
        {
            File= file;
		}

		public string File { get; private set; }
	}
}