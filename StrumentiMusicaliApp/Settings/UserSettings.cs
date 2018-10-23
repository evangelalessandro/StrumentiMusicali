using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Settings
{
    public class UserSettings
    {

		public List<Tuple<enAmbienti, FormRicerca>> Form { get; set; }

	}
	public class FormRicerca
	{
		public string LastStringaRicerca { get; set; }
		public Size SizeFormMain { get; set; } = new Size(400, 400);
		public string LastItemSelected { get; set; }
		public FormWindowState FormMainWindowState { get; set; } = FormWindowState.Maximized;
	}
	public enum enAmbienti
	{
		Main,
		Fatture,
		FattureList,
		Articoli,
		Magazzino
	}
}