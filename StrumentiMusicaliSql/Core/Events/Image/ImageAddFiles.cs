using StrumentiMusicali.Library.Core.Events.Base;
using System;
using System.Collections.Generic;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageAddFiles : FilterEvent
    {
        public ImageAddFiles(List<string> files, Guid key)
            : base(key)
        {

            Files = files;
        }

        public List<string> Files { get; private set; }
    }
}
