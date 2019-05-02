using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.View.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Settings
{
	public class UserSettings
	{
		public List<( enAmbiente ambiente, FormRicerca form)> Form { get; set; }
	}

	public class FormRicerca
	{
	//	public string LastStringaRicerca { get; set; } = "";
		public Size SizeFormMain { get; set; } = new Size(400, 400);
		public int LastItemSelected { get; set; }
		public FormWindowState FormMainWindowState { get; set; } = FormWindowState.Maximized;
		public FormStartPosition StartPosition { get; set; }
		public int Top { get; set; }
		public int Left { get; set; }
	}


}