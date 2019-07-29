using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StrumentiMusicali.Library.Entity
{
	public class SettingBackupFtp :Base.BaseEntity
    {
		[MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string UrlHost { get; set; }
        [MaxLength(100)]
        public string BaseFolder { get; set; } = "/htdocs/Extra/";

        public BackupSetting BackupSetting { get; set; } = new BackupSetting();
    }

    public class BackupSetting
    {
        [MaxLength(200)]
        public string FolderLocalServer { get; set; }

        [MaxLength(200)]
        public string NetworkFolder { get; set; }
    }
}