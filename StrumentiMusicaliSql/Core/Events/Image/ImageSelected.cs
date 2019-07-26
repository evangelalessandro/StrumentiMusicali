using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageSelected<T> : FilterEvent
        where T : Entity.Base.BaseEntity
    {
		public ImageSelected(ImmaginiFile<T> file, System.Guid key)
            : base(key)
        {
            File= file;
		}

		public ImmaginiFile<T> File { get; private set; }
	}
}