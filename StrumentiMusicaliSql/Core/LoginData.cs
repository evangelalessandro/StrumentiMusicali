using StrumentiMusicali.Library.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core
{
	public static class LoginData
	{
		public static void SetUtente(Utente utente)
		{
			utenteLogin = utente;
		}
		public static Utente utenteLogin { get; private set; }
	}
}
