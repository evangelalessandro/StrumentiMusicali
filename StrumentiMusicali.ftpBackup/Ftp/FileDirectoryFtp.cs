using System;

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
