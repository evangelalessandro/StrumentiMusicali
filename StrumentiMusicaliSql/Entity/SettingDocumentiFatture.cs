using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StrumentiMusicali.Library.Entity
{
	public class SettingDocumentiPagamenti
    {
		[Key]
		public int ID { get; set; } = 1;
        [MaxLength(200)]
 
        public string CartellaReteDocumentiPagamenti { get; set; }
        


    }
}