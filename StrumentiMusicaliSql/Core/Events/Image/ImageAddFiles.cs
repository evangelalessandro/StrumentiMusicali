using StrumentiMusicali.Library.Core.Events.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageAddFiles :FilterEvent
    {
        public ImageAddFiles(List<string> files, Guid key)
            :base(key)
        {
            
            Files = files;
        }

        public List<string> Files { get; private set; }
    }
}
