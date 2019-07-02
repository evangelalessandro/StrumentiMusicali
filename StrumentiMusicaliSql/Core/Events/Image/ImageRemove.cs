﻿using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageRemove : FilterEvent
    {
		public ImageRemove(string file, System.Guid key)
            : base(key)
        {
            File= file;
		}

		public string File { get; private set; }
	}
}