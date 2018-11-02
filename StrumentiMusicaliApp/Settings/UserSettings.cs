using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Settings
{
	public class UserSettings
	{
		public List<Tuple<enAmbienti, FormRicerca>> Form { get; set; }
		public DatiMittente datiMittente { get; set; } = new DatiMittente();
		public SettingSito settingSito { get; set; } = new SettingSito();
		public DatiIntestazioneStampaFattura DatiIntestazione { get; set; }
	}

	public class FormRicerca
	{
		public string LastStringaRicerca { get; set; } = "";
		public Size SizeFormMain { get; set; } = new Size(400, 400);
		public string LastItemSelected { get; set; }
		public FormWindowState FormMainWindowState { get; set; } = FormWindowState.Maximized;
		public FormStartPosition StartPosition { get; set; }
		public int Top { get; set; }
		public int Left { get; set; }
	}

	public enum enAmbienti
	{
		Main,
		Fattura,
		FattureList,
		Articolo,
		ArticoliList,
		Magazzino,
		SettingFatture,
		SettingSito,
		SettingStampa,
		ScaricoMagazzino,
		LogView,
		LogViewList,
		ClientiList,
		Cliente,
		FattureRigheList,
		FattureRigheDett,
	}
}