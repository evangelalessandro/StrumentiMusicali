using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StrumentiMusicali.Library.Entity
{
	public class SettingBackupFtp
    {
		[Key]
		public int ID { get; set; } = 1;

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string UrlHost { get; set; }
        [MaxLength(50)]
        public string BaseFolder { get; set; } = "/htdocs/Extra/";

    }
}