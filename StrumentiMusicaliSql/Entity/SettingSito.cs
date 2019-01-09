using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StrumentiMusicali.Library.Entity
{
	public class SettingSito
	{
		[Key]
		public int ID { get; set; } = 1;
		public string UrlSito { get; set; }
		public string UrlCompletaImmagini { get; set; }

		public string CartellaLocaleImmagini { get; set; }

		public string SoloNomeFileMercatino { get; set; }
		public string SoloNomeFileEcommerce { get; set; }

		public string UrlCompletoFileMercatino { get; set; }
		public string UrlCompletoFileEcommerce { get; set; }

		 
	}
}