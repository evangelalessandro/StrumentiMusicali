using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StrumentiMusicali.Library.Entity
{
	public class SettingSito : Base.BaseEntity
    {
		 
        [MaxLength(200)]

        public string UrlSito { get; set; }
        [MaxLength(200)]

        public string UrlCompletaImmagini { get; set; }
        [MaxLength(200)]

        public string CartellaLocaleImmagini { get; set; }
        
        [MaxLength(200)]

        public string SoloNomeFileMercatino { get; set; }
        [MaxLength(200)]

        public string SoloNomeFileEcommerce { get; set; }
        [MaxLength(200)]

        public string UrlCompletoFileMercatino { get; set; }
        [MaxLength(200)]
        public string UrlCompletoFileEcommerce { get; set; }

		 
	}
}