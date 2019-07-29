using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.ftpBackup.Ftp
{
    public class FileDirectoryFtp
    {
        public bool IsDirectory { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public long Size { get; set; }

        public DateTime CreatedData { get; set; }
    }
}
