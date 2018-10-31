using StrumentiMusicali.App.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Settings
{
	public class SettingSito
	{
		public string UrlSito { get; set; }
		public string UrlCompletaImmagini { get; set; }

		public string CartellaLocaleImmagini { get; set; }

		public string SoloNomeFileMercatino { get; set; }
		public string SoloNomeFileEcommerce { get; set; }

		public string UrlCompletoFileMercatino { get; set; }
		public string UrlCompletoFileEcommerce { get; set; }

		/// <summary>
		/// Controllo la cartella locale per le immagini se è correttamente settata e attiva
		/// </summary>
		/// <returns></returns>
		public bool CheckFolderImmagini()
		{
			var item = CartellaLocaleImmagini;
			if (string.IsNullOrEmpty(item))
			{
				MessageManager.NotificaWarnig("Occorre impostare la cartella per le immagini in locale!");
				return false;
			}
			if (!Directory.Exists(item))
			{
				MessageManager.NotificaWarnig(string.Format("La cartella per le immagini in locale specificata {0} non esiste o non è raggiungibile!", item));
				return false;
			}
			return true;
		}
	}
}
